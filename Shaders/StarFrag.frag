uniform sampler2D currentTexture; // Our render texture
uniform sampler2D distortionMapTexture; // Our heat distortion map texture

uniform float time; // Time used to scroll the distortion map
uniform float distortionFactor = .02f; // Factor used to control severity of the effect
uniform float riseFactor = .1f; // Factor used to control how fast air rises

void main()
{
    vec2 distortionMapCoordinate = gl_TexCoord[0].st;
    vec2 distortionMapCoordinate2 = gl_TexCoord[0].st;

    distortionMapCoordinate.t -= time * riseFactor;

    vec4 distortionMapValue = texture2D(distortionMapTexture, distortionMapCoordinate);

    distortionMapCoordinate2.s += time * 1.05 * riseFactor;

    vec4 distortionMapValue2 = texture2D(distortionMapTexture, distortionMapCoordinate2);

    vec2 distortionPositionOffset = distortionMapValue.xy;
    distortionPositionOffset -= vec2(0.5f, 0.5f);
    distortionPositionOffset *= 2.f;
    distortionPositionOffset *= distortionFactor;

    // The latter 2 channels of the texture are unused... be creative
    vec2 distortionPositionOffset2 = distortionMapValue2.xy;
    distortionPositionOffset2 -= vec2(0.5f, 0.5f);
    distortionPositionOffset2 *= 2.f;
    distortionPositionOffset2 *= distortionFactor;

    distortionPositionOffset *= (1.f - gl_TexCoord[0].t);
    distortionPositionOffset2 *= (1.f - gl_TexCoord[0].t);

    vec2 distortedTextureCoordinate = gl_TexCoord[0].st + distortionPositionOffset + distortionPositionOffset2;

    vec4 color = texture2D(currentTexture, distortedTextureCoordinate);
    float ratio = color.a * color.a;

    gl_FragColor = gl_Color * (1 - ratio) + ratio * vec4(1, 1, 1, 1);
    gl_FragColor.a = color.a;
}
