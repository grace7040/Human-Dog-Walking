using UnityEngine;

public static partial class QuaternionExtensions {
    public static Quaternion GetRelativeRotationFrom(this Quaternion rotation, Matrix4x4 from)
    {
        return from.GetRotation() * rotation;
    }

    public static Quaternion GetRelativeRotationTo(this Quaternion rotation, Matrix4x4 to)
    {
        return Quaternion.Inverse(to.GetRotation()) * rotation;
    }


    public static Quaternion GetMirror(this Quaternion quaternion, Vector3 axis)
    {
        Quaternion mirror = quaternion;
        if (axis == Vector3.right)
        {
            mirror.x *= -1f;
            mirror.w *= -1f;
        }
        if (axis == Vector3.up)
        {
            mirror.y *= -1f;
            mirror.w *= -1f;
        }
        if (axis == Vector3.forward)
        {
            mirror.z *= -1f;
            mirror.w *= -1f;
        }
        return Quaternion.Slerp(quaternion, mirror, 1f);
    }


    public static Vector3 GetLog(this Quaternion rotation)
    {
        //Quaternion exp_w = rotation.GetNormalised();
        //Quaternion w = 
        /*
		Vector3 log = Vector3.zero;
		float mag = rotation.GetMagnitude();
		float arg = (float)System.Math.Atan2(mag, rotation.w) / mag;
		log.x = rotation.x * arg;
		log.y = rotation.y * arg;
		log.z = rotation.z * arg;
		return log;
		*/

        /*
      double b = sqrt(q.x*q.x + q.y*q.y + q.z*q.z);
      if(fabs(b) <= _QUATERNION_EPS*fabs(q.w)) {
        if(q.w<0.0) {
          // fprintf(stderr, "Input quaternion(%.15g, %.15g, %.15g, %.15g) has no unique logarithm; returning one arbitrarily.", q.w, q.x, q.y, q.z);
          if(fabs(q.w+1)>_QUATERNION_EPS) {
            quaternion r = {log(-q.w), M_PI, 0., 0.};
            return r;
          } else {
            quaternion r = {0., M_PI, 0., 0.};
            return r;
          }
        } else {
          quaternion r = {log(q.w), 0., 0., 0.};
          return r;
        }
      } else {
        double v = atan2(b, q.w);
        double f = v/b;
        quaternion r = { log(q.w*q.w+b*b)/2.0, f*q.x, f*q.y, f*q.z };
        return r;
      }
        */

        Quaternion q = rotation.GetNormalised();
        float b = q.GetMagnitude();
        float v = Mathf.Atan2(b, q.w);
        float f = v / b;
        Quaternion r = new Quaternion(f * q.x, f * q.y, f * q.z, Mathf.Log(q.w * q.w + b * b) / 2f);

        Vector3 log = new Vector3(r.x, r.y, r.z);
        log.x = Mathf.Abs(log.x);
        log.y = Mathf.Abs(log.y);
        log.z = Mathf.Abs(log.z);

        return log;
    }

    public static Quaternion GetExp(this Vector3 rotation)
    {
        float w = (float)System.Math.Sqrt(rotation.x * rotation.x + rotation.y * rotation.y + rotation.z * rotation.z);
        Quaternion exp = Quaternion.identity;
        exp.x = rotation.x * (float)System.Math.Sin(w) / w;
        exp.y = rotation.y * (float)System.Math.Sin(w) / w;
        exp.z = rotation.z * (float)System.Math.Sin(w) / w;
        exp.w = (float)System.Math.Cos(w);
        return exp;//exp.GetNormalised();
    }
    public static Quaternion LookRotationXZ(Vector3 right, Vector3 forward) {
		return Quaternion.LookRotation(right, forward) * Quaternion.Euler(-90f, 0f, -90f); //forward-to-up
	}

	public static Quaternion LookRotationXY(Vector3 right, Vector3 up) {
		return Quaternion.LookRotation(right, up) * Quaternion.Euler(0f, -90f, 0f); //forward-to-up
	}

	public static Quaternion RotationFrom(this Quaternion rotation, Matrix4x4 from) {
		return from.GetRotation() * rotation;
	}

	public static Quaternion RotationTo(this Quaternion rotation, Matrix4x4 to) {
		return Quaternion.Inverse(to.GetRotation()) * rotation;
	}

	public static Quaternion RotationFromTo(this Quaternion rotation, Matrix4x4 from, Matrix4x4 to) {
		return rotation.RotationTo(from).RotationFrom(to);
	}

	public static Quaternion RotationFrom(this Quaternion rotation, Quaternion from) {
		return from * rotation;
	}

	public static Quaternion RotationTo(this Quaternion rotation, Quaternion to) {
		return Quaternion.Inverse(to) * rotation;
	}

	public static Quaternion FromTo(this Quaternion rotation, Quaternion from, Quaternion to) {
		return rotation.RotationTo(from).RotationFrom(to);
	}

	public static Vector3 GetRight(this Quaternion quaternion) {
		return quaternion * Vector3.right;
	}

	public static Vector3 GetUp(this Quaternion quaternion) {
		return quaternion * Vector3.up;
	}

	public static Vector3 GetForward(this Quaternion quaternion) {
		return quaternion * Vector3.forward;
	}
	
	public static Vector4 GetVector(this Quaternion quaternion) {
		return new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
	}

	public static Quaternion GetMirror(this Quaternion quaternion, Axis axis) {
		Quaternion mirror = quaternion;
		if(axis == Axis.XPositive) {
			mirror.x *= -1f;
			mirror.w *= -1f;
		}
		if(axis == Axis.YPositive) {
			mirror.y *= -1f;
			mirror.w *= -1f;
		}
		if(axis == Axis.ZPositive) {
			mirror.z *= -1f;
			mirror.w *= -1f;
		}
		return Quaternion.Slerp(quaternion, mirror, 1f);
	}

	public static Quaternion LookRotation(Vector3 forward, Vector3 up) {
		Vector3 cross = Vector3.Cross(forward, up);
		Vector3 mean = ((forward + up) / 2f).normalized;
		forward = Quaternion.AngleAxis(-45f, cross) * mean;
		up = Quaternion.AngleAxis(45f, cross) * mean;
		return Quaternion.LookRotation(forward, up);
	}

	public static Quaternion GetNormalised(this Quaternion rotation) {
		float length = rotation.GetMagnitude();
		rotation.x /= length;
		rotation.y /= length;
		rotation.z /= length;
		rotation.w /= length;
		return rotation;
	}

	public static float GetMagnitude(this Quaternion rotation) {
		return Mathf.Sqrt(rotation.x*rotation.x + rotation.y*rotation.y + rotation.z*rotation.z + rotation.w*rotation.w);
	}

	public static Quaternion GetInverse(this Quaternion rotation) {
		return Quaternion.Inverse(rotation);
	}

	public static Quaternion Mean(this Quaternion[] quaternions) {
		if(quaternions.Length == 0) {
			return Quaternion.identity;
		}
		if(quaternions.Length == 1) {
			return quaternions[0];
		}
		if(quaternions.Length == 2) {
			return Quaternion.Slerp(quaternions[0], quaternions[1], 0.5f);
		}
		Vector3 forward = Vector3.zero;
		Vector3 upwards = Vector3.zero;
		for(int i=0; i<quaternions.Length; i++) {
			forward += quaternions[i] * Vector3.forward;
			upwards += quaternions[i] * Vector3.up;
		}
		forward /= quaternions.Length;
		upwards /= quaternions.Length;
		return Quaternion.LookRotation(forward, upwards);
	}

	public static Quaternion Mean(this Quaternion[] quaternions, float[] weights) {
		if(quaternions.Length == 0) {
			return Quaternion.identity;
		}
		if(quaternions.Length == 1) {
			return quaternions[0];
		}
		if(quaternions.Length != weights.Length) {
			Debug.Log("Failed to compute mean because size of vectors and weights does not match.");
			return Quaternion.identity;
		}
		float sum = 0f;
		Vector3 forwards = Vector3.zero;
		Vector3 upwards = Vector3.zero;
		for(int i=0; i<quaternions.Length; i++) {
			forwards += weights[i] * (quaternions[i] * Vector3.forward);
			upwards += weights[i] * (quaternions[i] * Vector3.up);
			sum += weights[i];
		}
		// forwards /= quaternions.Length;
		// upwards /= quaternions.Length;
		if(sum == 0f) {
			Debug.Log("Failed to compute mean because size of sum of weights is zero.");
			return Quaternion.identity;
		}
		return Quaternion.LookRotation((forwards/sum).normalized, (upwards/sum).normalized);
	}

	private static Vector3[] GaussianForwards = null;
	private static Vector3[] GaussianUpwards = null;
	public static Quaternion Gaussian(this Quaternion[] values, float power=1f, bool[] mask=null) {
		if(values.Length == 0) {
			return Quaternion.identity;
		}
		if(values.Length == 1) {
			return values[0];
		}
		GaussianForwards = GaussianForwards.Validate(values.Length);
		GaussianUpwards = GaussianUpwards.Validate(values.Length);
		for(int i=0; i<values.Length; i++) {
			GaussianForwards[i] = values[i].GetForward();
			GaussianUpwards[i] = values[i].GetUp();
		}
		return Quaternion.LookRotation(GaussianForwards.Gaussian(power, mask).normalized, GaussianUpwards.Gaussian(power, mask).normalized);
	}

	private static Vector3[] MaskedGaussianForwards = null;
	private static Vector3[] MaskedGaussianUpwards = null;
	public static Quaternion Gaussian(this Quaternion[] values, bool[] mask, float power=1f) {
		if(values.Length == 0) {
			return Quaternion.identity;
		}
		if(values.Length == 1) {
			return values[0];
		}
		MaskedGaussianForwards = MaskedGaussianForwards.Validate(values.Length);
		MaskedGaussianUpwards = MaskedGaussianUpwards.Validate(values.Length);
		for(int i=0; i<values.Length; i++) {
			MaskedGaussianForwards[i] = values[i].GetForward();
			MaskedGaussianUpwards[i] = values[i].GetUp();
		}
		return Quaternion.LookRotation(MaskedGaussianForwards.Gaussian(power, mask).normalized, MaskedGaussianUpwards.Gaussian(power, mask).normalized);
	}

	public static float[] ToArray(this Quaternion rotation) {
		return new float[4]{rotation.x, rotation.y, rotation.z, rotation.w};
	}

	public static float[][] ToArray(this Quaternion[] rotations) {
		float[][] values = new float[rotations.Length][];
		for(int i=0; i<values.Length; i++) {
			values[i] = rotations[i].ToArray();
		}
		return values;
	}

	public static Vector3[] GetForwards(this Quaternion[] rotations) {
		Vector3[] values = new Vector3[rotations.Length];
		for(int i=0; i<values.Length; i++) {
			values[i] = rotations[i].GetForward();
		}
		return values;
	}

	public static Vector3[] GetUps(this Quaternion[] rotations) {
		Vector3[] values = new Vector3[rotations.Length];
		for(int i=0; i<values.Length; i++) {
			values[i] = rotations[i].GetUp();
		}
		return values;
	}

	public static Vector3[] GetRights(this Quaternion[] rotations) {
		Vector3[] values = new Vector3[rotations.Length];
		for(int i=0; i<values.Length; i++) {
			values[i] = rotations[i].GetRight();
		}
		return values;
	}

}
