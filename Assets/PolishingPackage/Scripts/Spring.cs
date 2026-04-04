using UnityEngine;
using UnityEngine.InputSystem;

/*
 * For more info on springs:
 * Toyful Games Spring Tutorial: https://www.youtube.com/watch?v=bFOAipGJGA0
 * https://www.joshwcomeau.com/animation/a-friendly-introduction-to-spring-physics/
 * https://en.wikipedia.org/wiki/Harmonic_oscillator#Damped_harmonic_oscillator
 */

namespace Polishing
{
    public class Spring
    {
        public float stiffness;
        // 0 means infinite back and forth oscillation, 1 means perfect damping to stop at goal
        public float damping;
        
        public float position = 0.0f;
        public float velocity = 0.0f;
        public float goalPosition = 0.0f;

        public Spring(float stiffness, float damping, float goalPosition)
        {
            this.stiffness = stiffness;
            this.damping = damping;
            this.goalPosition = goalPosition;
            position = goalPosition;
        }
        
        public void Simulate(float delta)
        {
            float positionDifference = position - goalPosition;
            float springForce = -positionDifference * stiffness;
            float dampingForce = -velocity * damping * 2.0f * Mathf.Sqrt(stiffness);
            velocity += (springForce + dampingForce) * delta;
            position += velocity * delta;
        }

        public void Reset()
        {
            position = goalPosition;
            velocity = 0.0f;
        }

        public void NudgeSpring(float amount)
        {
            velocity += amount;
        }
    }
}
