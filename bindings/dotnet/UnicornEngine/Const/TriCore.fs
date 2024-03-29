// For Unicorn Engine. AUTO-GENERATED FILE, DO NOT EDIT

namespace UnicornEngine.Const

open System

[<AutoOpen>]
module TriCore =

    // TRICORE CPU

    let UC_CPU_TRICORE_TC1796 = 0
    let UC_CPU_TRICORE_TC1797 = 1
    let UC_CPU_TRICORE_TC27X = 2
    let UC_CPU_TRICORE_ENDING = 3

    // TRICORE registers

    let UC_TRICORE_REG_INVALID = 0
    let UC_TRICORE_REG_A0 = 1
    let UC_TRICORE_REG_A1 = 2
    let UC_TRICORE_REG_A2 = 3
    let UC_TRICORE_REG_A3 = 4
    let UC_TRICORE_REG_A4 = 5
    let UC_TRICORE_REG_A5 = 6
    let UC_TRICORE_REG_A6 = 7
    let UC_TRICORE_REG_A7 = 8
    let UC_TRICORE_REG_A8 = 9
    let UC_TRICORE_REG_A9 = 10
    let UC_TRICORE_REG_A10 = 11
    let UC_TRICORE_REG_A11 = 12
    let UC_TRICORE_REG_A12 = 13
    let UC_TRICORE_REG_A13 = 14
    let UC_TRICORE_REG_A14 = 15
    let UC_TRICORE_REG_A15 = 16
    let UC_TRICORE_REG_D0 = 17
    let UC_TRICORE_REG_D1 = 18
    let UC_TRICORE_REG_D2 = 19
    let UC_TRICORE_REG_D3 = 20
    let UC_TRICORE_REG_D4 = 21
    let UC_TRICORE_REG_D5 = 22
    let UC_TRICORE_REG_D6 = 23
    let UC_TRICORE_REG_D7 = 24
    let UC_TRICORE_REG_D8 = 25
    let UC_TRICORE_REG_D9 = 26
    let UC_TRICORE_REG_D10 = 27
    let UC_TRICORE_REG_D11 = 28
    let UC_TRICORE_REG_D12 = 29
    let UC_TRICORE_REG_D13 = 30
    let UC_TRICORE_REG_D14 = 31
    let UC_TRICORE_REG_D15 = 32
    let UC_TRICORE_REG_PCXI = 33
    let UC_TRICORE_REG_PSW = 34
    let UC_TRICORE_REG_PSW_USB_C = 35
    let UC_TRICORE_REG_PSW_USB_V = 36
    let UC_TRICORE_REG_PSW_USB_SV = 37
    let UC_TRICORE_REG_PSW_USB_AV = 38
    let UC_TRICORE_REG_PSW_USB_SAV = 39
    let UC_TRICORE_REG_PC = 40
    let UC_TRICORE_REG_SYSCON = 41
    let UC_TRICORE_REG_CPU_ID = 42
    let UC_TRICORE_REG_BIV = 43
    let UC_TRICORE_REG_BTV = 44
    let UC_TRICORE_REG_ISP = 45
    let UC_TRICORE_REG_ICR = 46
    let UC_TRICORE_REG_FCX = 47
    let UC_TRICORE_REG_LCX = 48
    let UC_TRICORE_REG_COMPAT = 49
    let UC_TRICORE_REG_DPR0_U = 50
    let UC_TRICORE_REG_DPR1_U = 51
    let UC_TRICORE_REG_DPR2_U = 52
    let UC_TRICORE_REG_DPR3_U = 53
    let UC_TRICORE_REG_DPR0_L = 54
    let UC_TRICORE_REG_DPR1_L = 55
    let UC_TRICORE_REG_DPR2_L = 56
    let UC_TRICORE_REG_DPR3_L = 57
    let UC_TRICORE_REG_CPR0_U = 58
    let UC_TRICORE_REG_CPR1_U = 59
    let UC_TRICORE_REG_CPR2_U = 60
    let UC_TRICORE_REG_CPR3_U = 61
    let UC_TRICORE_REG_CPR0_L = 62
    let UC_TRICORE_REG_CPR1_L = 63
    let UC_TRICORE_REG_CPR2_L = 64
    let UC_TRICORE_REG_CPR3_L = 65
    let UC_TRICORE_REG_DPM0 = 66
    let UC_TRICORE_REG_DPM1 = 67
    let UC_TRICORE_REG_DPM2 = 68
    let UC_TRICORE_REG_DPM3 = 69
    let UC_TRICORE_REG_CPM0 = 70
    let UC_TRICORE_REG_CPM1 = 71
    let UC_TRICORE_REG_CPM2 = 72
    let UC_TRICORE_REG_CPM3 = 73
    let UC_TRICORE_REG_MMU_CON = 74
    let UC_TRICORE_REG_MMU_ASI = 75
    let UC_TRICORE_REG_MMU_TVA = 76
    let UC_TRICORE_REG_MMU_TPA = 77
    let UC_TRICORE_REG_MMU_TPX = 78
    let UC_TRICORE_REG_MMU_TFA = 79
    let UC_TRICORE_REG_BMACON = 80
    let UC_TRICORE_REG_SMACON = 81
    let UC_TRICORE_REG_DIEAR = 82
    let UC_TRICORE_REG_DIETR = 83
    let UC_TRICORE_REG_CCDIER = 84
    let UC_TRICORE_REG_MIECON = 85
    let UC_TRICORE_REG_PIEAR = 86
    let UC_TRICORE_REG_PIETR = 87
    let UC_TRICORE_REG_CCPIER = 88
    let UC_TRICORE_REG_DBGSR = 89
    let UC_TRICORE_REG_EXEVT = 90
    let UC_TRICORE_REG_CREVT = 91
    let UC_TRICORE_REG_SWEVT = 92
    let UC_TRICORE_REG_TR0EVT = 93
    let UC_TRICORE_REG_TR1EVT = 94
    let UC_TRICORE_REG_DMS = 95
    let UC_TRICORE_REG_DCX = 96
    let UC_TRICORE_REG_DBGTCR = 97
    let UC_TRICORE_REG_CCTRL = 98
    let UC_TRICORE_REG_CCNT = 99
    let UC_TRICORE_REG_ICNT = 100
    let UC_TRICORE_REG_M1CNT = 101
    let UC_TRICORE_REG_M2CNT = 102
    let UC_TRICORE_REG_M3CNT = 103
    let UC_TRICORE_REG_ENDING = 104
    let UC_TRICORE_REG_GA0 = 1
    let UC_TRICORE_REG_GA1 = 2
    let UC_TRICORE_REG_GA8 = 9
    let UC_TRICORE_REG_GA9 = 10
    let UC_TRICORE_REG_SP = 11
    let UC_TRICORE_REG_LR = 12
    let UC_TRICORE_REG_IA = 16
    let UC_TRICORE_REG_ID = 32

