uniform sampler2D currentTexture; // Our render texture

uniform float time; // Time used to scroll the distortion map

uniform vec2 points[20];
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

    vec2 origin = vec2(-100000, -10000);

    float minDist = 100000;


    int nbIntersect = 0;
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

        float num1 = point1.x * point2.y - point1.y * point2.x;
        float num2 = origin.x * coordinate.y - origin.y * coordinate.x;
        float denum = (point1.x - point2.x) * (origin.y - coordinate.y) - (point1.y - point2.y) * (origin.x - coordinate.x);

        if(denum != 0)
        {
            float intersecX = (num1 * (origin.x - coordinate.x) - num2 * (point1.x - point2.x)) / denum;   
            float intersecY = (num1 * (origin.y - coordinate.y) - num2 * (point1.y - point2.y)) / denum;

            vec2 intersect = vec2(intersecX, intersecY);

            if(intersect == point2)
            {

            }
            else if(dot(intersect - point1, intersect - point2) < 0 && dot(intersect - origin, intersect - coordinate) < 0)
            {
                nbIntersect++;
            }
        }

        vec2 firstVector = coordinate - point1;
        //vec2 vector2 = coordinate - point2;
        vec2 normalizedEdge = normalize(point2 - point1);
        //vec2 vector = vector - normalizedEdge * dot(normalizedEdge, vector);

        vec3 crossVector = cross(vec3(firstVector, 0), vec3(normalizedEdge, 0));
        //vec3 crossVector2 = cross(vec3(vector2, 0), vec3(-normalizedEdge, 0));

        vec2 secondVector = coordinate - point2;
        if(dot(normalizedEdge, firstVector) * dot(normalizedEdge, secondVector) < 0)
        {
            float crossLen = abs(crossVector.z);

            if(crossLen < minDist)
            {
                minDist = crossLen;
            }
        }

        float lenToPoint = length(firstVector);
        if(lenToPoint < minDist)
        {
            minDist = lenToPoint;
        }

    }  

    if(isFilled)
    {
        if(nbIntersect % 2 == 1)
        {
            alphaRatio = 0.5;
        }
    }

    if(minDist < margin / 2)
    {
        alphaRatio = 0.5;
    }

    return alphaRatio;
}