sampler2D g_samSrcColor;

float4 StarColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color;
    
    Color = tex2D( g_samSrcColor, Tex.xy);
	if (Color.r == (246.0f/255.0f) && Color.g == (246.0f/255.0f) && Color.b == (246.0f/255.0f))
	{
		Color.r = ((StarColor.r * 30) + 225) / 255.0f;
		Color.g = ((StarColor.g * 30) + 225) / 255.0f;
		Color.b = ((StarColor.b * 30) + 225) / 255.0f;
		return Color;
	}
	if (Color.r == (225.0f/255.0f) && Color.g == (225.0f/255.0f) && Color.b == (225.0f/255.0f))
	{
		Color.r = ((StarColor.r * 100) + 155) / 255.0f;
		Color.g = ((StarColor.g * 100) + 155) / 255.0f;
		Color.b = ((StarColor.b * 100) + 155) / 255.0f;
		return Color;
	}
	if (Color.r == (205.0f/255.0f) && Color.g == (193.0f/255.0f) && Color.b == (208.0f/255.0f))
	{
		Color.r = ((StarColor.r * 150) + 55) / 255.0f;
		Color.g = ((StarColor.g * 150) + 55) / 255.0f;
		Color.b = ((StarColor.b * 150) + 55) / 255.0f;
		return Color * 0.95f;
	}
	if (Color.r == (154.0f/255.0f) && Color.g == (152.0f/255.0f) && Color.b == (156.0f/255.0f))
	{
		return StarColor * 0.65f;
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