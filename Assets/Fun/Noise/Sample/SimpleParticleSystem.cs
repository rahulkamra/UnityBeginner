using UnityEngine;
using System.Collections;

public class SimpleParticleSystem
{
    private ParticleSystem.Particle[] particles;
    private ParticleSystem particleSystem;

    private Rect bounds;
    public void Init(ParticleSystem particleSystem,int rows , int cols)
    {
        this.particleSystem = particleSystem;
        bounds = new Rect((float)-rows/2, (float)-cols/2, rows, cols);
    }
   

	
	// Update is called once per frame
	public void Update (Vector3[,] flowField , int rows,int cols , float velocityMul)
    {
        if (particles == null)
            return;
        
        for (int idx = 0; idx < particles.Length; idx++)
        {
            Vector3 clamp;
            ParticleSystem.Particle eachParticle = particles[idx];

            clamp = eachParticle.position;


            if(clamp.x < bounds.xMin)
            {
                clamp.x = Mathf.Floor(bounds.xMax - (bounds.xMin - clamp.x));
            }

            if (clamp.x > bounds.xMax)
            {
                clamp.x = Mathf.Floor(bounds.xMin + (bounds.xMax - clamp.x));
            }


            if(clamp.y < bounds.yMin)
            {
                clamp.y = Mathf.Floor(bounds.yMax - (bounds.yMin - clamp.y));
            }

            if (clamp.y > bounds.yMax)
            {
                clamp.y = Mathf.Floor(bounds.yMin + (bounds.yMax - clamp.y));
            }

          //  eachParticle.position = clamp;

            //now we need to re map the position of this particles position to the actual flow field 

            int col = (int)NoiseSampleTextureCreator.Remap(clamp.x, bounds.xMin, bounds.xMax, 0, cols-1);
            int row = (int)NoiseSampleTextureCreator.Remap(clamp.y, bounds.yMin, bounds.yMax, 0, rows-1);
            
            Vector3 velocity = flowField[row, col];
            clamp = clamp + velocity * velocityMul;
            eachParticle.position = clamp;

            particles[idx] = eachParticle;
        }

        particleSystem.SetParticles(particles,particles.Length);

    }

    public void InitParticles(int NumParticles)
    {
        particles = new ParticleSystem.Particle[NumParticles];
        for (int ii = 0; ii < particles.Length; ++ii)
        {
            particles[ii].position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.xMin, bounds.xMax), 0);
            particles[ii].color = Color.black;
            particles[ii].size = 1f;
           // particles[ii].velocity = new Vector3(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f), 0);
        }
    }

    
}
