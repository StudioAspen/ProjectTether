#ifndef ALLIN1VFXSRPBATCH_BUFFERSCONFIG
#define ALLIN1VFXSRPBATCH_BUFFERSCONFIG

BATCHING_BUFFER_START
	#include "ShaderLibrary/AllIn1VfxSRPBatch_PropertiesDeclaration.hlsl"
BATCHING_BUFFER_END

#if defined(UNITY_DOTS_INSTANCING_ENABLED)
	UNITY_DOTS_INSTANCING_START(UserPropertyMetadata)
		#include "ShaderLibrary/AllIn1VfxSRPBatch_PropertiesDeclaration.hlsl"
	UNITY_DOTS_INSTANCING_END(UserPropertyMetadata)
#endif

#include "ShaderLibrary/AllIn1VfxSRPBatch_GlobalPropertiesAndTexturesDeclaration.hlsl"

#endif //ALLIN1VFXSRPBATCH_BUFFERSCONFIG