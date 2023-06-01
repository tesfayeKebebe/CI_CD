#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
219,
227,
228,
229,
230,
231,
232,
233,
234,
235,
238,
239,
411,
412,
414,
442,
443,
444,
464,
465,
466,
467,
565,
566,
567,
570,
607,
608,
609,
612,
614,
616,
618,
623,
631,
632,
633,
634,
635,
636,
637,
638,
639,
640,
641,
642,
643,
644,
645,
646,
647,
649,
650,
651,
652,
653,
654,
655,
749,
750,
751,
752,
753,
754,
755,
756,
757,
758,
759,
760,
761,
762,
763,
764,
765,
767,
768,
769,
770,
771,
772,
773,
901,
910,
913,
915,
920,
921,
923,
924,
928,
929,
931,
932,
935,
936,
937,
940,
942,
945,
947,
949,
1020,
1022,
1024,
1033,
1034,
1035,
1036,
1038,
1045,
1046,
1047,
1048,
1049,
1057,
1058,
1059,
1063,
1064,
1066,
1070,
1071,
1072,
1343,
1543,
1544,
8915,
8916,
8918,
8919,
8920,
8921,
8922,
8924,
8926,
8928,
8929,
8930,
8939,
8941,
8947,
8948,
8950,
8952,
8954,
8965,
8974,
8975,
8977,
8978,
8979,
8980,
8981,
8983,
8985,
10107,
10111,
10113,
10114,
10115,
10116,
10262,
10263,
10264,
10265,
10285,
10286,
10287,
10289,
10332,
10407,
10409,
10420,
10421,
10422,
10423,
10866,
10867,
10872,
10873,
10910,
10941,
10947,
10954,
10964,
10968,
11057,
11059,
11072,
11074,
11075,
11076,
11083,
11096,
11116,
11117,
11125,
11127,
11134,
11135,
11138,
11140,
11145,
11151,
11152,
11159,
11161,
11173,
11176,
11177,
11178,
11189,
11198,
11204,
11205,
11206,
11208,
11209,
11227,
11229,
11243,
11267,
11268,
11269,
11287,
11292,
11322,
11323,
11799,
11800,
11801,
11880,
11968,
12266,
12267,
12274,
12275,
12276,
12282,
12354,
12814,
12815,
13343,
13344,
14287,
14308,
14315,
14317,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementType_raw (int,int);
int ves_icall_System_Array_IsValueOfElementType_raw (int,int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy_raw (int,int,int,int,int,int);
int ves_icall_System_Array_GetLength_raw (int,int,int);
int ves_icall_System_Array_GetLowerBound_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
int ves_icall_System_Array_GetValueImpl_raw (int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
int ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
int ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw (int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int mono_object_hash_icall_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
void ves_icall_AssemblyExtensions_ApplyUpdate (int,int,int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 219,
ves_icall_System_Array_InternalCreate,
// token 227,
ves_icall_System_Array_GetCorElementTypeOfElementType_raw,
// token 228,
ves_icall_System_Array_IsValueOfElementType_raw,
// token 229,
ves_icall_System_Array_CanChangePrimitive,
// token 230,
ves_icall_System_Array_FastCopy_raw,
// token 231,
ves_icall_System_Array_GetLength_raw,
// token 232,
ves_icall_System_Array_GetLowerBound_raw,
// token 233,
ves_icall_System_Array_GetGenericValue_icall,
// token 234,
ves_icall_System_Array_GetValueImpl_raw,
// token 235,
ves_icall_System_Array_SetGenericValue_icall,
// token 238,
ves_icall_System_Array_SetValueImpl_raw,
// token 239,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 411,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 412,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 414,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 442,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 443,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 444,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 464,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 465,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 466,
ves_icall_System_Enum_InternalGetCorElementType,
// token 467,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 565,
ves_icall_System_Environment_get_ProcessorCount,
// token 566,
ves_icall_System_Environment_get_TickCount,
// token 567,
ves_icall_System_Environment_get_TickCount64,
// token 570,
ves_icall_System_Environment_FailFast_raw,
// token 607,
ves_icall_System_GC_GetCollectionCount,
// token 608,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 609,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 612,
ves_icall_System_GC_SuppressFinalize_raw,
// token 614,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 616,
ves_icall_System_GC_GetGCMemoryInfo,
// token 618,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 623,
ves_icall_System_Object_MemberwiseClone_raw,
// token 631,
ves_icall_System_Math_Acos,
// token 632,
ves_icall_System_Math_Acosh,
// token 633,
ves_icall_System_Math_Asin,
// token 634,
ves_icall_System_Math_Asinh,
// token 635,
ves_icall_System_Math_Atan,
// token 636,
ves_icall_System_Math_Atan2,
// token 637,
ves_icall_System_Math_Atanh,
// token 638,
ves_icall_System_Math_Cbrt,
// token 639,
ves_icall_System_Math_Ceiling,
// token 640,
ves_icall_System_Math_Cos,
// token 641,
ves_icall_System_Math_Cosh,
// token 642,
ves_icall_System_Math_Exp,
// token 643,
ves_icall_System_Math_Floor,
// token 644,
ves_icall_System_Math_Log,
// token 645,
ves_icall_System_Math_Log10,
// token 646,
ves_icall_System_Math_Pow,
// token 647,
ves_icall_System_Math_Sin,
// token 649,
ves_icall_System_Math_Sinh,
// token 650,
ves_icall_System_Math_Sqrt,
// token 651,
ves_icall_System_Math_Tan,
// token 652,
ves_icall_System_Math_Tanh,
// token 653,
ves_icall_System_Math_FusedMultiplyAdd,
// token 654,
ves_icall_System_Math_Log2,
// token 655,
ves_icall_System_Math_ModF,
// token 749,
ves_icall_System_MathF_Acos,
// token 750,
ves_icall_System_MathF_Acosh,
// token 751,
ves_icall_System_MathF_Asin,
// token 752,
ves_icall_System_MathF_Asinh,
// token 753,
ves_icall_System_MathF_Atan,
// token 754,
ves_icall_System_MathF_Atan2,
// token 755,
ves_icall_System_MathF_Atanh,
// token 756,
ves_icall_System_MathF_Cbrt,
// token 757,
ves_icall_System_MathF_Ceiling,
// token 758,
ves_icall_System_MathF_Cos,
// token 759,
ves_icall_System_MathF_Cosh,
// token 760,
ves_icall_System_MathF_Exp,
// token 761,
ves_icall_System_MathF_Floor,
// token 762,
ves_icall_System_MathF_Log,
// token 763,
ves_icall_System_MathF_Log10,
// token 764,
ves_icall_System_MathF_Pow,
// token 765,
ves_icall_System_MathF_Sin,
// token 767,
ves_icall_System_MathF_Sinh,
// token 768,
ves_icall_System_MathF_Sqrt,
// token 769,
ves_icall_System_MathF_Tan,
// token 770,
ves_icall_System_MathF_Tanh,
// token 771,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 772,
ves_icall_System_MathF_Log2,
// token 773,
ves_icall_System_MathF_ModF,
// token 901,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 910,
ves_icall_RuntimeType_make_array_type_raw,
// token 913,
ves_icall_RuntimeType_make_byref_type_raw,
// token 915,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 920,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 921,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 923,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 924,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 928,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 929,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 931,
ves_icall_System_RuntimeType_getFullName_raw,
// token 932,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 935,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 936,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 937,
ves_icall_RuntimeType_GetFields_native_raw,
// token 940,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 942,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 945,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 947,
ves_icall_RuntimeType_GetName_raw,
// token 949,
ves_icall_RuntimeType_GetNamespace_raw,
// token 1020,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1022,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1024,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1033,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1034,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1035,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1036,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1038,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1045,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1046,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1047,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1048,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1049,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1057,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1058,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1059,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1063,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1064,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1066,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1070,
ves_icall_System_String_FastAllocateString_raw,
// token 1071,
ves_icall_System_String_InternalIsInterned_raw,
// token 1072,
ves_icall_System_String_InternalIntern_raw,
// token 1343,
ves_icall_System_Type_internal_from_handle_raw,
// token 1543,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1544,
ves_icall_System_ValueType_Equals_raw,
// token 8915,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 8916,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 8918,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 8919,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 8920,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 8921,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 8922,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 8924,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 8926,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 8928,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 8929,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 8930,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 8939,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 8941,
mono_monitor_exit_icall_raw,
// token 8947,
ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw,
// token 8948,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 8950,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 8952,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 8954,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 8965,
ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw,
// token 8974,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 8975,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 8977,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 8978,
ves_icall_System_Threading_Thread_GetState_raw,
// token 8979,
ves_icall_System_Threading_Thread_SetState_raw,
// token 8980,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 8981,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 8983,
ves_icall_System_Threading_Thread_YieldInternal,
// token 8985,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 10107,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 10111,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 10113,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 10114,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 10115,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 10116,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 10262,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 10263,
ves_icall_System_GCHandle_InternalFree_raw,
// token 10264,
ves_icall_System_GCHandle_InternalGet_raw,
// token 10265,
ves_icall_System_GCHandle_InternalSet_raw,
// token 10285,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 10286,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 10287,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 10289,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 10332,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 10407,
mono_object_hash_icall_raw,
// token 10409,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 10420,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 10421,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 10422,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 10423,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 10866,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 10867,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 10872,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 10873,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 10910,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 10941,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 10947,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 10954,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 10964,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 10968,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 11057,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 11059,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 11072,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 11074,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 11075,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 11076,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 11083,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 11096,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 11116,
ves_icall_reflection_get_token_raw,
// token 11117,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 11125,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 11127,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 11134,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 11135,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 11138,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 11140,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 11145,
ves_icall_reflection_get_token_raw,
// token 11151,
ves_icall_get_method_info_raw,
// token 11152,
ves_icall_get_method_attributes,
// token 11159,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 11161,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 11173,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 11176,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 11177,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 11178,
ves_icall_reflection_get_token_raw,
// token 11189,
ves_icall_InternalInvoke_raw,
// token 11198,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 11204,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 11205,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 11206,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 11208,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 11209,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 11227,
ves_icall_InvokeClassConstructor_raw,
// token 11229,
ves_icall_InternalInvoke_raw,
// token 11243,
ves_icall_reflection_get_token_raw,
// token 11267,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 11268,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 11269,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 11287,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 11292,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 11322,
ves_icall_reflection_get_token_raw,
// token 11323,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 11799,
ves_icall_AssemblyExtensions_ApplyUpdate,
// token 11800,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 11801,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 11880,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 11968,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 12266,
ves_icall_ModuleBuilder_basic_init_raw,
// token 12267,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 12274,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 12275,
ves_icall_ModuleBuilder_getToken_raw,
// token 12276,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 12282,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 12354,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 12814,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 12815,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 13343,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 13344,
ves_icall_System_Diagnostics_Debugger_Log,
// token 14287,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 14308,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 14315,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 14317,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_handles [] = {
0,
1,
1,
0,
1,
1,
1,
0,
1,
0,
1,
1,
0,
0,
0,
1,
1,
1,
1,
1,
0,
1,
0,
0,
0,
1,
0,
1,
1,
1,
1,
0,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
};
