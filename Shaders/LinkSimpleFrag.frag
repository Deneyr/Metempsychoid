uniform sampler2D currentTexture; // Our render texture
uniform sampler2D distTexture; // Our render texture

uniform float ratioFrom;
uniform float ratioTo;
uniform bool isActive;

uniform float widthRatio;

uniform float time; // Time used to scroll the distortion map
uniform float distortionFactor = .02f; // Factor used to control severity of the effect
uniform float riseFactor = .05f; // Factor used to control how fast air rises

float GetAlphaRatio(vec2 coordinate);

void main()
{
    vec2 coordinate = gl_TexCoord[0].st;

    float alphaRatio = GetAlphaRatio(coordinate);

    vec2 coordinateTexture = vec2(coordinate.s, coordinate.t - time * riseFactor);
    vec2 coordinateTexture2 = vec2(coordinate.s + 0.2, coordinate.t + time * riseFactor);

    coordinate.s += time * riseFactor * 0.7;
    coordinate.t -= time * riseFactor * 0.7;

    vec4 colorTexture1 = texture2D(currentTexture, coordinateTexture);
    vec4 colorTexture2 = texture2D(distTexture, coordinateTexture2);

    vec4 colorDistortion = texture2D(currentTexture, coordinate);

    vec4 colorTexture = (colorTexture1 + colorTexture2);

    float alphaScalar = 1 - abs(gl_TexCoord[0].t * 10 * 2 - 1);

    float ratio = alphaScalar * colorTexture.r * 0.8;

    float ratioDist = colorDistortion.r * ratio;

    ratio -= (1 - ratioDist) * 0.5;

    gl_FragColor = gl_Color * (1 - ratio * 0.6) + ratio * 0.6 * vec4(1, 1, 1, 1);
    gl_FragColor.a = ratio * alphaRatio;
}

float GetAlphaRatio(vec2 coordinate)
{
    float pixelRatioFrom = coordinate.s / widthRatio;
    float pixelRatioTo = (widthRatio - coordinate.s) / widthRatio;

    if(isActive){
        if(pixelRatioFrom < ratioFrom
        || pixelRatioTo < ratioTo)
        {
            return 1;
        }

        float returnValue = 0;
        if(pixelRatioFrom > ratioFrom
        && pixelRatioFrom - ratioFrom < 0.1)
        {
            returnValue = max(returnValue, 1 - (pixelRatioFrom - ratioFrom) / 0.1);
        }

        if(pixelRatioTo > ratioTo
        && pixelRatioTo - ratioTo < 0.1)
        {
            returnValue = max(returnValue, 1 - (pixelRatioTo - ratioTo) / 0.1);
        }

        return returnValue;
    }else{

        float invRatioFrom = 0.5 - ratioFrom;
        float invRatioTo = 0.5 - ratioTo;

        if(pixelRatioFrom > invRatioFrom
        && pixelRatioTo > invRatioTo){
            return 1;
        }

        if(invRatioFrom > pixelRatioFrom
        && invRatioFrom - pixelRatioFrom < 0.1)
        {
            return 1 - (invRatioFrom - pixelRatioFrom) / 0.1;
        }

        if(invRatioTo > pixelRatioTo
        && invRatioTo - pixelRatioTo < 0.1)
        {
            return 1 - (invRatioTo - pixelRatioTo) / 0.1;
        }
        return 0;
    }
} 
