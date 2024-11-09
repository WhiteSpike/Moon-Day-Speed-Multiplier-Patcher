using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MoonDaySpeedMultiplierPatcher.Util
{
    internal static class Tools
    {
        public static void FindCodeInstruction(ref int index, ref List<CodeInstruction> codes, object findValue, MethodInfo addCode, bool skip = false, bool requireInstance = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], findValue)) continue;
                found = true;
                if (skip) break;
                if (andInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.And));
                if (!andInstruction && orInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Or));
                if (notInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Not));
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.mls.LogError(errorMessage);
            index++;
        }
        public static void FindLocalField(ref int index, ref List<CodeInstruction> codes, int localIndex, object addCode = null, bool skip = false, bool store = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], localIndex, store)) continue;
                found = true;
                if (skip) break;
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.mls.LogError(errorMessage);
            index++;
        }
        public static void FindString(ref int index, ref List<CodeInstruction> codes, string findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindField(ref int index, ref List<CodeInstruction> codes, FieldInfo findField, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findField, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindMethod(ref int index, ref List<CodeInstruction> codes, MethodInfo findMethod, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findMethod, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindFloat(ref int index, ref List<CodeInstruction> codes, float findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindInteger(ref int index, ref List<CodeInstruction> codes, sbyte findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: findValue, addCode: addCode, skip: skip, requireInstance: requireInstance, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, errorMessage: errorMessage);
        }
        public static void FindSub(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Sub, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindDiv(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Div, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindAdd(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Add, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        public static void FindMul(ref int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            FindCodeInstruction(ref index, ref codes, findValue: OpCodes.Mul, addCode: addCode, skip: skip, notInstruction: notInstruction, andInstruction: andInstruction, orInstruction: orInstruction, requireInstance: requireInstance, errorMessage: errorMessage);
        }
        private static bool CheckCodeInstruction(CodeInstruction code, int localIndex, bool store = false)
        {
            if (!store)
            {
                switch (localIndex)
                {
                    case 0: return code.opcode == OpCodes.Ldloc_0;
                    case 1: return code.opcode == OpCodes.Ldloc_1;
                    case 2: return code.opcode == OpCodes.Ldloc_2;
                    case 3: return code.opcode == OpCodes.Ldloc_3;
                    default:
                        {
                            return (code.opcode == OpCodes.Ldloc || code.opcode == OpCodes.Ldloc_S) && code.operand is LocalBuilder local && local.LocalIndex == localIndex;
                        }
                }
            }
            else
            {
                switch (localIndex)
                {
                    case 0: return code.opcode == OpCodes.Stloc_0;
                    case 1: return code.opcode == OpCodes.Stloc_1;
                    case 2: return code.opcode == OpCodes.Stloc_2;
                    case 3: return code.opcode == OpCodes.Stloc_3;
                    default:
                        {
                            return (code.opcode == OpCodes.Stloc || code.opcode == OpCodes.Stloc_S) && code.operand is LocalBuilder local && local.LocalIndex == localIndex;
                        }
                }
            }
        }
        private static bool CheckCodeInstruction(CodeInstruction code, object findValue)
        {
            if (findValue is sbyte)
            {
                bool result = CheckIntegerCodeInstruction(code, findValue);
                return result;
            }
            if (findValue is float)
            {
                bool result = code.opcode == OpCodes.Ldc_R4 && code.operand.Equals(findValue);
                return result;
            }
            if (findValue is string) return code.opcode == OpCodes.Ldstr && code.operand.Equals(findValue);
            if (findValue is MethodInfo) return (code.opcode == OpCodes.Call || code.opcode == OpCodes.Callvirt) && code.operand == findValue;
            if (findValue is FieldInfo)
            {
                return (code.opcode == OpCodes.Ldfld || code.opcode == OpCodes.Stfld) && code.operand == findValue;
            }
            if (findValue is OpCode) return code.opcode == (OpCode)findValue;
            return false;
        }
        private static bool CheckIntegerCodeInstruction(CodeInstruction code, object findValue)
        {
            switch ((sbyte)findValue)
            {
                case 0: return code.opcode == OpCodes.Ldc_I4_0;
                case 1: return code.opcode == OpCodes.Ldc_I4_1;
                case 2: return code.opcode == OpCodes.Ldc_I4_2;
                case 3: return code.opcode == OpCodes.Ldc_I4_3;
                case 4: return code.opcode == OpCodes.Ldc_I4_4;
                case 5: return code.opcode == OpCodes.Ldc_I4_5;
                case 6: return code.opcode == OpCodes.Ldc_I4_6;
                case 7: return code.opcode == OpCodes.Ldc_I4_7;
                case 8: return code.opcode == OpCodes.Ldc_I4_8;
                default:
                    {
                        return code.opcode == OpCodes.Ldc_I4_S && code.operand.Equals(findValue);
                    }
            }
        }
    }
}
