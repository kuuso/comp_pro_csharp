using System;

struct mlong {
	const long mod = (long) 1e9 + 7;
	long V;
	
	public mlong(long _v = 0){
		V = _v;
	}
	
	public static mlong operator+(mlong a, mlong b){
		var v0 = a.V + b.V; if(v0 >= mod) v0 -= mod;
		return new mlong(v0);
	}
	public static mlong operator-(mlong a, mlong b){
		var v0 = mod + a.V - b.V; if(v0 >= mod) v0 -= mod;
		return new mlong(v0);
	}
	public static mlong operator-(mlong b){
		var v0 = mod - b.V; if(v0 >= mod) v0 -= mod;
		return new mlong(v0);
	}
	public static mlong operator*(mlong a, mlong b){
		var v0 = a.V * b.V; if(v0 >= mod) v0 %= mod;
		return new mlong(v0);
	}
	public static mlong operator/(mlong a, mlong b){
		var v0 = a.V * inv(b.V).V; if(v0 >= mod) v0 %= mod;
		return new mlong(v0);
	}
	
	public static mlong inv(long x){
		long a = 0, b = 0, c = 0;
		ExtGCD(x, mod, ref a, ref b, ref c);
		return (mlong)((a + mod) % mod);
	}
	
	public static void ExtGCD(long x, long y, ref long a, ref long b, ref long c){
		long r0 = x; long r1 = y;
		long a0 = 1; long a1 = 0;
		long b0 = 0; long b1 = 1;
		long q1, r2, a2, b2;
		while(r1 > 0){
			q1 = r0 / r1;
			r2 = r0 % r1;
			a2 = a0 - q1 * a1;
			b2 = b0 - q1 * b1;
			r0 = r1; r1 = r2;
			a0 = a1; a1 = a2;
			b0 = b1; b1 = b2;
		}
		c = r0;
		a = a0;
		b = b0;
	}
	
	public static mlong ModPow(mlong a, long k){
		if(k == 0) return (mlong) 1;
		if(a == 0) return (mlong) 0;
		mlong x = a;
		mlong ret = 1;
		while(k > 0){
			if(k % 2 == 1) ret *= x;
			x *= x;
			k >>= 1;
		}
		return ret;
	}
	
	
	public static bool operator == (mlong a, mlong b){
		 return a.Equals(b);
	}
	public static bool operator != (mlong a, mlong b){
		 return !(a == b);
	}
	
	public override bool Equals(System.Object obj){
		if(obj  ==  null) return false;
		mlong p  =  (mlong) obj;
		if((System.Object) p  ==  null) return false;
		return p.V  ==  V;
	}
	public override int GetHashCode(){
		return V.GetHashCode();
	}
	
	public static implicit operator mlong(long n){
		long v = n % mod; if(v < 0) v += mod;
		return new mlong(v);
	}
	public static implicit operator mlong(int n){
		long v = n % mod; if(v < 0) v += mod;
		return new mlong(v);
	}
	public static explicit operator long(mlong n){
		return n.V;
	}
	
	public override String ToString(){
		return V.ToString();
	}
	
}


class Test{
	static void Main(){
		
		mlong x = -2;
		mlong y = -1;
		mlong z = -2;
		Console.WriteLine("x:{0} y:{1} z:{2}", x, y, z);
		Console.WriteLine("x + y: {0}", x + y);
		Console.WriteLine("x - y: {0}", x - y);
		Console.WriteLine("x * y: {0}", x * y);
		Console.WriteLine("x / y: {0}", x / y);
		Console.WriteLine("x == y: {0}", x == y);
		Console.WriteLine("x != y: {0}", x != y);
		Console.WriteLine("x == z: {0}", x == z);
		Console.WriteLine("x != z: {0}", x != z);
		Console.WriteLine("(long) x: {0}", (long) x);
		Console.WriteLine("x == -2: {0}", x == -2);
		
		for(int i=0;i<10;i++){
			Console.WriteLine("modPow(-2, {0}): {1}",i, mlong.ModPow(-2, i));
		}
		
	}
}
