namespace Everglow.Myth.TheFirefly.Physics;

internal class Spring
{
	/// <summary>
	/// 弹簧的弹性常数
	/// </summary>
	public float elasticity;

	/// <summary>
	/// 弹簧在不受外力的情况下的长度
	/// </summary>
	public float restLength;

	/// <summary>
	/// 弹簧受到的阻力 d(v)
	/// </summary>
	public float damping;

	/// <summary>
	/// 弹簧端点上的两个质点
	/// </summary>
	public Mass mass1;

	public Mass mass2;

	public Spring(float elasticity, float restLength, float damping, Mass m1, Mass m2)
	{
		this.elasticity = elasticity;
		this.restLength = restLength;
		mass1 = m1;
		mass2 = m2;
		this.damping = damping;
	}

	private void ForceSingleDirection(Mass m1, Mass m2, float deltaTime)
	{
		// 求解阻尼简谐运动的微分方程解析解
		if (4 * elasticity - damping * damping <= 0)
			return;
		float gamma = 0.5f * MathF.Sqrt(4 * elasticity - damping * damping);
		var unit = Vector2.Normalize(m1.position - m2.position);
		Vector2 dir = m1.position - m2.position - unit * restLength;
		Vector2 c = dir * (damping / (2 * gamma)) + m1.velocity * (1.0f / gamma);
		Vector2 target = dir * MathF.Cos(gamma * deltaTime) + c * MathF.Sin(gamma * deltaTime);
		target *= MathF.Exp(-0.5f * deltaTime * damping);
		Vector2 acc = (target - dir) * (1.0f / deltaTime / deltaTime) - m1.velocity * (1.0f / deltaTime);

		m1.force += acc;

		//float dis = (m2.position - m1.position).Length();
		//Vector2 n = Vector2.Normalize(m2.position - m1.position);
		//Vector2 acc = n * (dis - restLength) * (elasticity);
		//m1.force += acc * m1.mass;
	}

	public void ApplyForce(float deltaTime)
	{
		ForceSingleDirection(mass1, mass2, deltaTime);
		ForceSingleDirection(mass2, mass1, deltaTime);
	}
}