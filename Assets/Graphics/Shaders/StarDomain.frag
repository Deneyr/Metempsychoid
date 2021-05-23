uniform sampler2D currentTexture; // Our render texture

uniform float time; // Time used to scroll the distortion map

uniform vec2[20] points;
uniform int pointsLen = 20;

uniform float margin;
uniform bool isFilled;

float GetAlphaRatio(vec2 coordinate);

void main()
{
    vec2 distortionMapCoordinate = gl_TexCoord[0].st;
    vec2 distortionMapCoordinate2 = gl_TexCoord[0].st;

    distortionMapCoordinate.t -= time * 0.1;
    vec4 distortionMapValue = texture2D(currentTexture, distortionMapCoordinate);

    distortionMapCoordinate2.s += time * 0.1;
    vec4 distortionMapValue2 = texture2D(currentTexture, distortionMapCoordinate2);

    vec2 distortedTextureCoordinate = gl_TexCoord[0].st + (vec2(distortionMapValue.r, distortionMapValue2.g) - vec2(1, 1)) * 0.05;

    gl_FragColor = gl_Color;
    gl_FragColor.a *= GetAlphaRatio(distortedTextureCoordinate);
}

float GetAlphaRatio(vec2 coordinate)
{
    float alphaRatio = 0;
    float lenOutside = 10000;
    bool outsideFound = false;
    bool isInside = true;

    for(int i = 0; i < pointsLen; i++)
    {
        vec2 point1 = points[i];
        vec2 point2; 
        if(i == pointsLen - 1)
        {
            point2 = points[0];
        }
        else
        {
            point2 = points[i + 1];
        }    

        vec2 currentVector = coordinate - point1;
        vec2 currentEdge = normalize(point2 - point1);

        vec3 crossVector = cross(vec3(currentVector, 0), vec3(currentEdge, 0));

        /*if(crossVector.z > 0)
        {
            isInside = false;

            vec2 secondVector = coordinate - point2;

            if(dot(currentEdge, currentVector) * dot(currentEdge, secondVector) < 0)
            {
                lenOutside = crossVector.z;
                outsideFound = true;
            }
        }*/

        if(crossVector.z > 0)
        {
            isInside = false;
        }

        vec2 secondVector = coordinate - point2;

        if(dot(currentEdge, currentVector) * dot(currentEdge, secondVector) < 0)
        {
            float crossLen = abs(crossVector.z);

            if(lenOutside > crossLen)
            {
                lenOutside = crossLen;
            }
        }

    }  

    /*if(outsideFound == false)
    {
        bool firstTime = true;
        for(int i = 0; i < pointsLen; i++)
        {
            vec2 currentVector = coordinate - points[i];
            float lenVec = length(currentVector);
            if(firstTime || lenVec < lenOutside)
            {
                lenOutside = lenVec;
                firstTime = false;
            }
        }
    }*/

    for(int i = 0; i < pointsLen; i++)
    {
        vec2 currentVector = coordinate - points[i];
        float lenVec = length(currentVector);
        if(lenVec < lenOutside)
        {
            lenOutside = lenVec;
        }
    }

    /*if(isInside)
    {
        alphaRatio = 0.7;
    }
    else
    {
        alphaRatio = min(0.7, 2 * (margin - lenOutside) / margin);
    }*/
    
    if((isFilled && isInside) || lenOutside < margin / 2)
    {
        alphaRatio = 0.5;
    }
    else
    {
        alphaRatio = 0;
    }

    //alphaRatio = min(0.7, max((margin - lenOutside), 0) / margin);

    return alphaRatio;
}