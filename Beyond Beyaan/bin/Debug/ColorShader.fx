sampler2D g_samSrcColor;

float4 EmpireColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color;
    
    Color = tex2D( g_samSrcColor, Tex.xy);
	if (Color.r == 1 && Color.g == (165.0f/255.0f) && Color.b == (99.0f/255.0f))
	{
		return EmpireColor;
	}
	if (Color.r == (244.0f/255.0f) && Color.g == (74.0f/255.0f) && Color.b == (52.0f/255.0f))
	{
		return EmpireColor * 0.8f;
	}
	if (Color.r == (181.0f/255.0f) && Color.g == (13.0f/255.0f) && Color.b == (13.0f/255.0f))
	{
		return EmpireColor * 0.55f;
	}
	return Color;
}


technique PostProcess
{
    pass p1
    {
        VertexShader = null;
        PixelShader = compile ps_2_0 MyShader();
    }

}
