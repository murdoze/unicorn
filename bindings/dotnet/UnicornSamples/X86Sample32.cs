﻿using Gee.External.Capstone;
using Gee.External.Capstone.X86;
using System;
using System.Diagnostics;
using System.Text;
using UnicornEngine;
using UnicornEngine.Const;

namespace UnicornSamples
{
    internal static class X86Sample32
    {
        private const long ADDRESS = 0x1000000;

        public static void X86Code32()
        {
            byte[] X86_CODE32 =
            {
                // INC ecx; DEC edx
                0x41, 0x4a
            };
            Run(X86_CODE32);
        }

        public static void X86Code32InvalidMemRead()
        {
            byte[] X86_CODE32_MEM_READ =
            {
                // mov ecx,[0xaaaaaaaa]; INC ecx; DEC edx
                0x8B, 0x0D, 0xAA, 0xAA, 0xAA, 0xAA, 0x41, 0x4a
            };
            Run(X86_CODE32_MEM_READ);
        }

        public static void X86Code32InvalidMemWriteWithRuntimeFix()
        {
            byte[] X86_CODE32_MEM_WRITE =
            {
                // mov [0xaaaaaaaa], ecx; INC ecx; DEC edx
                0x89, 0x0D, 0xAA, 0xAA, 0xAA, 0xAA, 0x41, 0x4a
            };
            Run(X86_CODE32_MEM_WRITE);
        }

        public static void X86Code32InOut()
        {
            byte[] X86_CODE32_INOUT =
            {
                // INC ecx; IN AL, 0x3f; DEC edx; OUT 0x46, AL; INC ebx
                0x41, 0xE4, 0x3F, 0x4a, 0xE6, 0x46, 0x43
            };
            Run(X86_CODE32_INOUT);
        }


        private static void Run(byte[] code, bool raiseException = false)
        {
            Console.WriteLine();
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrames()[1];
            var methodName = stackFrame.GetMethod()?.Name;

            Console.WriteLine($"*** Start: {methodName}");
            Exception e = null;
            try
            {
                RunTest(code, ADDRESS, Common.UC_MODE_32);
            }
            catch (UnicornEngineException ex)
            {
                e = ex;
            }

            if (!raiseException && e != null)
            {
                Console.Error.WriteLine("Emulation FAILED! " + e.Message);
            }

            Console.WriteLine("*** End: " + methodName);
            Console.WriteLine();
        }

        private static void RunTest(byte[] code, long address, int mode)
        {
            using var u = new Unicorn(Common.UC_ARCH_X86, mode);
            using var disassembler = CapstoneDisassembler.CreateX86Disassembler(X86DisassembleMode.Bit32);
            Console.WriteLine($"Unicorn version: {u.Version()}");

            // map 2MB of memory for this emulation
            u.MemMap(address, 2 * 1024 * 1024, Common.UC_PROT_ALL);

            // initialize machine registers
            u.RegWrite(X86.UC_X86_REG_EAX, 0x1234);
            u.RegWrite(X86.UC_X86_REG_ECX, 0x1234);
            u.RegWrite(X86.UC_X86_REG_EDX, 0x7890);

            // write machine code to be emulated to memory
            u.MemWrite(address, code);

            // initialize machine registers
            u.RegWrite(X86.UC_X86_REG_ESP, Utils.Int64ToBytes(address + 0x200000));

            // handle IN & OUT instruction
            u.AddInHook(InHookCallback);
            u.AddOutHook(OutHookCallback);

            // tracing all instructions by having @begin > @end
            u.AddCodeHook((uc, addr, size, userData) => CodeHookCallback(disassembler, uc, addr, size, userData), 1, 0);

            // handle interrupt ourself
            u.AddInterruptHook(InterruptHookCallback);

            // handle SYSCALL
            u.AddSyscallHook(SyscallHookCallback);

            // intercept invalid memory events
            u.AddEventMemHook(MemMapHookCallback, Common.UC_HOOK_MEM_READ_UNMAPPED | Common.UC_HOOK_MEM_WRITE_UNMAPPED);

            Console.WriteLine(">>> Start tracing code");

            // emulate machine code in infinite time
            u.EmuStart(address, address + code.Length, 0u, 0u);

            // print registers
            var ecx = u.RegRead(X86.UC_X86_REG_ECX);
            var edx = u.RegRead(X86.UC_X86_REG_EDX);
            var eax = u.RegRead(X86.UC_X86_REG_EAX);
            Console.WriteLine($"[!] EAX = {eax:X}");
            Console.WriteLine($"[!] ECX = {ecx:X}");
            Console.WriteLine($"[!] EDX = {edx:X}");

            Console.WriteLine(">>> Emulation Done!");
        }

        private static int InHookCallback(Unicorn u, int port, int size, object userData)
        {
            var eip = u.RegRead(X86.UC_X86_REG_EIP);
            Console.WriteLine($"[!] Reading from port 0x{port:X}, size: {size:X}, address: 0x{eip:X}");
            var res = size switch
            {
                1 =>
                    // read 1 byte to AL
                    0xf1,
                2 =>
                    // read 2 byte to AX
                    0xf2,
                4 =>
                    // read 4 byte to EAX
                    0xf4,
                _ => 0
            };

            Console.WriteLine($"[!] Return value: {res:X}");
            return res;
        }

        private static void OutHookCallback(Unicorn u, int port, int size, int value, object userData)
        {
            var eip = u.RegRead(X86.UC_X86_REG_EIP);
            Console.WriteLine($"[!] Writing to port 0x{port:X}, size: {size:X}, value: 0x{value:X}, address: 0x{eip:X}");

            // confirm that value is indeed the value of AL/ AX / EAX
            var v = 0L;
            var regName = string.Empty;
            switch (size)
            {
                case 1:
                    // read 1 byte in AL
                    v = u.RegRead(X86.UC_X86_REG_AL);
                    regName = "AL";
                    break;
                case 2:
                    // read 2 byte in AX
                    v = u.RegRead(X86.UC_X86_REG_AX);
                    regName = "AX";
                    break;
                case 4:
                    // read 4 byte in EAX
                    v = u.RegRead(X86.UC_X86_REG_EAX);
                    regName = "EAX";
                    break;
            }

            Console.WriteLine("[!] Register {0}: {1:X}", regName, v);
        }

        private static bool MemMapHookCallback(Unicorn u, int eventType, long address, int size, long value, object userData)
        {
            if (eventType != Common.UC_MEM_WRITE_UNMAPPED) return false;

            Console.WriteLine($"[!] Missing memory is being WRITE at 0x{address:X}, data size = {size:X}, data value = 0x{value:X}. Map memory.");
            u.MemMap(0xaaaa0000, 2 * 1024 * 1024, Common.UC_PROT_ALL);

            return true;
        }

        private static void CodeHookCallback1(
            CapstoneX86Disassembler disassembler,
            Unicorn u,
            long addr,
            int size,
            object userData)
        {
            Console.Write($"[+] 0x{addr:X}: ");

            var eipBuffer = new byte[4];
            u.RegRead(X86.UC_X86_REG_EIP, eipBuffer);

            var effectiveSize = Math.Min(16, size);
            var tmp = new byte[effectiveSize];
            u.MemRead(addr, tmp);

            var sb = new StringBuilder();
            foreach (var t in tmp)
            {
                sb.AppendFormat($"{(0xFF & t):X} ");
            }
            Console.Write($"{sb,-20}");
            Console.WriteLine(Utils.Disassemble(disassembler, tmp));
        }

        private static void CodeHookCallback(
            CapstoneX86Disassembler disassembler,
            Unicorn u,
            long addr,
            int size,
            object userData)
        {
            Console.Write($"[+] 0x{addr:X}: ");

            var eipBuffer = new byte[4];
            u.RegRead(X86.UC_X86_REG_EIP, eipBuffer);

            var effectiveSize = Math.Min(16, size);
            var tmp = new byte[effectiveSize];
            u.MemRead(addr, tmp);

            var sb = new StringBuilder();
            foreach (var t in tmp)
            {
                sb.AppendFormat($"{(0xFF & t):X} ");
            }
            Console.Write($"{sb,-20}");
            Console.WriteLine(Utils.Disassemble(disassembler, tmp));
        }

        private static void SyscallHookCallback(Unicorn u, object userData)
        {
            var eaxBuffer = new byte[4];
            u.RegRead(X86.UC_X86_REG_EAX, eaxBuffer);
            var eax = Utils.ToInt(eaxBuffer);

            Console.WriteLine($"[!] Syscall EAX = 0x{eax:X}");

            u.EmuStop();
        }

        private static void InterruptHookCallback(Unicorn u, int intNumber, object userData)
        {
            // only handle Linux syscall
            if (intNumber != 0x80)
            {
                return;
            }

            var eaxBuffer = new byte[4];
            var eipBuffer = new byte[4];

            u.RegRead(X86.UC_X86_REG_EAX, eaxBuffer);
            u.RegRead(X86.UC_X86_REG_EIP, eipBuffer);

            var eax = Utils.ToInt(eaxBuffer);
            var eip = Utils.ToInt(eipBuffer);

            switch (eax)
            {
                default:
                    Console.WriteLine($"[!] Interrupt 0x{eip:X} num {intNumber:X}, EAX=0x{eax:X}");
                    break;
                case 1: // sys_exit
                    Console.WriteLine($"[!] Interrupt 0x{eip:X} num {intNumber:X}, SYS_EXIT");
                    u.EmuStop();
                    break;
                case 4: // sys_write

                    // ECX = buffer address
                    var ecxBuffer = new byte[4];

                    // EDX = buffer size
                    var edxBuffer = new byte[4];

                    u.RegRead(X86.UC_X86_REG_ECX, ecxBuffer);
                    u.RegRead(X86.UC_X86_REG_EDX, edxBuffer);

                    var ecx = Utils.ToInt(ecxBuffer);
                    var edx = Utils.ToInt(edxBuffer);

                    // read the buffer in
                    var size = Math.Min(256, edx);
                    var buffer = new byte[size];
                    u.MemRead(ecx, buffer);
                    var content = Encoding.Default.GetString(buffer);

                    Console.WriteLine($"[!] Interrupt 0x{eip:X}: num {ecx:X}, SYS_WRITE. buffer = 0x{edx:X}, size = {size:X}, content = '{content}'");

                    break;
            }
        }
    }
}
