using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace CredECard.Common.BusinessService
{
    public class CredInvoker
    {
        object HostObject;
        object[] _valueParams = null;
        string _methodName = string.Empty;

        /// <summary>
        /// Constructor initialises target, methodname and parameter values
        /// </summary>
        /// <param name="target">object</param>
        /// <param name="methodName">string</param>
        /// <param name="valueParams">object[]</param>
        public CredInvoker(object target, string methodName, object[] valueParams)
        {
            _methodName = methodName;
            HostObject = target;
            _valueParams = valueParams;
        }
        
        /// <summary>
        /// Invokes the method given in constructor
        /// </summary>
        /// <returns>object</returns>
        public object Execute1()
        {
            // Get the Type for the class
            Type calledType = HostObject.GetType();

            try
            {
                //get method information which needs to be executed runtime.
                MethodInfo methodInf = calledType.GetMethod(_methodName);

                //execute method.
                return methodInf.Invoke(HostObject, _valueParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Involes the method with bindingflags 
        /// </summary>
        /// <returns></returns>
        public object Execute()
        {
            // Get the Type for the class
            Type calledType = HostObject.GetType();
            BindingFlags bflags = BindingFlags.Default | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance;

            return calledType.InvokeMember(_methodName, bflags | BindingFlags.InvokeMethod, null, HostObject, _valueParams);
        }

        /// <summary>
        /// Invokes given method with parameters
        /// </summary>
        /// <param name="target">object</param>
        /// <param name="methodName">string</param>
        /// <param name="valueParams">object[]</param>
        /// <returns>object</returns>
        public static object ExecuteMethod(object target, string methodName, object[] valueParams)
        {
            // Get the Type for the class
            Type calledType = target.GetType();

            try
            {
                //get method information which needs to be executed runtime.
                MethodInfo methodInf = calledType.GetMethod(methodName);

                //execute method.
                return methodInf.Invoke(target, valueParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #region otherway
    /*
    public delegate object CredInvokeHandler(object target, object[] valueParams);
    [Serializable]
    public class CredInvoker
    {
        CredInvokeHandler MyDelegate;
        MethodInfo MyMethodInfo;
        ParameterInfo[] MyParameters;
        object HostObject;
        int NumberOfArguments;
        object[] _valueParams = null;

        public CredInvoker(object target, string methodName, object[] valueParams)
        {
            HostObject = target;
            Type t2 = target.GetType();
            MethodInfo m2 = t2.GetMethod(methodName);
            MyDelegate = GetMethodInvoker(m2);
            NumberOfArguments = m2.GetParameters().Length;
            MyMethodInfo = m2;
            MyParameters = m2.GetParameters();
            _valueParams = valueParams;
        }

        public object Execute()
        {
            try
            {
                return (MyDelegate(HostObject, _valueParams));
            }
            catch (Exception)
            {
                throw;
                //Object o = new Object();
                //o = e.Message;
                //return (o);
            }

        }

        private CredInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            CredInvokeHandler invoder = (CredInvokeHandler)dynamicMethod.CreateDelegate(typeof(CredInvokeHandler));
            return invoder;
        }

        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
     */
    #endregion
}
