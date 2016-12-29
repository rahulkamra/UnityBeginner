using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SurfaceFlow : MonoBehaviour
{

    public SurfaceCreator Surface;

    private ParticleSystem system;
    private ParticleSystem.Particle[] particles;

    private void LateUpdate()
    {
        if(system == null)
        {
            system = GetComponent<ParticleSystem>();
        }

        if(particles == null || particles.Length < system.maxParticles)
        {
            particles = new ParticleSystem.Particle[system.maxParticles];
        }

        int particleCount = system.GetParticles(particles);
        positionParticles();
        system.SetParticles(particles, particleCount);
    }


    private void positionParticles()
    {
        Quaternion q = Quaternion.Euler(Surface.Rotation);
        Quaternion qInv = Quaternion.Inverse(q);

        SurfaceNoiseMethod method = SurfaceNoise.NoiseMethods[(int)Surface.MethodType][Surface.Dimention - 1];
        float amplitude = Surface.Damping ? Surface.Strength / Surface.Frequency : Surface.Strength;

        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 position = particles[i].position;
            Vector3 point = q * new Vector3(position.x,position.z) + Surface.Offset;

            NoiseSample sample = SurfaceNoise.Sum(method, point, Surface.Frequency, Surface.Octaves, Surface.Lacunarity, Surface.Persistence);
            if (Surface.MethodType == SurfaeNoiseMthodType.Value)
                sample = sample - 0.5f;
            else
                sample = sample * 0.5f;

            sample *= amplitude;
            sample.Derivative = qInv * sample.Derivative;
            position.y = sample.Value + system.startSize;
            particles[i].position = position;
        }
    }


}
