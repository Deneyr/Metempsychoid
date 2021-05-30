uniform sampler2D distortionMapTexture; // Our render texture

uniform float widthRatio;
uniform float heightRatio;

uniform vec4 playerColor;
uniform float ratioColor;
uniform bool isSocketed;

uniform bool isFocused;

uniform float margin;
uniform float outMargin;

uniform float time; // Time used to scroll the distortion map
uniform float distortionFactor = .02f; // Factor used to control severity of the effect
uniform float riseFactor = .1f; // Factor used to control how fast air rises

float GetMarginRatio(vec2 coordinate);

vec4 GetColor(vec2 coordinate, float ratio);

void main()
{
    vec2 canevasCoord = gl_TexCoord[0].st - vec2(outMargin + 0.005, outMargin + 0.005);
    vec2 distortionMapCoordinate = gl_TexCoord[0].st;

    distortionMapCoordinate.t -= time * riseFactor;
    distortionMapCoordinate.s += time * riseFactor * 1.1;

    vec4 distortionMapValue = texture2D(distortionMapTexture, distortionMapCoordinate);

    float marginRatio = GetMarginRatio(canevasCoord + distortionMapValue.xy * 0.010);

    vec4 color = GetColor(gl_TexCoord[0].st, marginRatio);

    float ratioFocused = 1;
    if(isFocused)
    {
        ratioFocused = (1 + sin(time * 10)) / 2;
    }

    gl_FragColor = (marginRatio) * color + (1 - marginRatio * 0.5) * vec4(0, 0, 0, 1) * ratioFocused + (1 - ratioFocused) * vec4(0.8, 0.8, 0.8, 1);
    gl_FragColor.a = marginRatio;
}

float GetMarginRatio(vec2 coordinate)
{
    float ratio;

    float realWidthRatio = widthRatio - 2 * outMargin;
    float realHeightRatio = heightRatio - 2 * outMargin;

    if(coordinate.s > margin
        && realWidthRatio - coordinate.s > margin
        && coordinate.t > margin
        && realHeightRatio - coordinate.t > margin)
    {
        ratio = 0;

        ratio = max(ratio, max(0, margin - abs(coordinate.s - margin)) / margin);

        ratio = max(ratio, max(0, margin - abs(realWidthRatio - coordinate.s - margin)) / margin);

        ratio = max(ratio, max(0, margin - abs(coordinate.t - margin)) / margin);

        ratio = max(ratio, max(0, margin - abs(realHeightRatio - coordinate.t - margin)) / margin);
    }
    else
    {
        ratio = 1;

        if(coordinate.s < margin)
        {
            ratio = min(ratio, max(0, margin - abs(coordinate.s - margin)) / margin);
        }
        else if(realWidthRatio - coordinate.s < margin)
        {
            ratio = min(ratio, max(0, margin - abs(realWidthRatio - coordinate.s - margin)) / margin);
        }

        if(coordinate.t < margin)
        {
            ratio = min(ratio, max(0, margin - abs(coordinate.t - margin)) / margin);
        }
        else if(realHeightRatio - coordinate.t < margin)
        {
            ratio = min(ratio, max(0, margin - abs(realHeightRatio - coordinate.t - margin)) / margin);
        }
    }

    return ratio;
}

vec4 GetColor(vec2 coordinate, float ratio)
{
    if(ratio < 0.01)
    {
        return gl_Color;
    }

    float ratioHeight = coordinate.t / heightRatio;

    vec4 colorFrom, colorTo;
    if(isSocketed)
    {
        colorFrom = gl_Color;
        colorTo = playerColor;
    }
    else
    {
        colorFrom = playerColor;
        colorTo = gl_Color;
    }

    if(ratioHeight < ratioColor)
    {
        return colorTo;
    }
    else if(ratioHeight < ratioColor + 0.2)
    {
        float ratioTransition =  (ratioHeight - ratioColor) / 0.2;
        return colorFrom * ratioTransition + colorTo * (1 - ratioTransition);
    }
    return colorFrom;
}
