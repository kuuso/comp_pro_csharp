using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BI = System.Numerics.BigInteger;
// for performance measurement
using System.Diagnostics;

public class Factorization{
	//static BI SieveSize = (BI) (Math.Pow(2.0, 61 / 3.0) + 10);
	static BI SieveSize = ((BI) 2) << 21 ;
	static BI UpperLimit = SieveSize * SieveSize * SieveSize;
	static BI SieveDivLimit = SieveSize * SieveSize - 1;
	static BI TrialDivSize = (BI) 2e5;
	static BI TrialDivLimit = TrialDivSize * TrialDivSize;
	
	bool[] isPrime;
	public List<BI> Primes;
	public Factorization(){
		isPrime = new bool[(int) SieveSize];
		Primes = new List<BI>();
		for(int i=2;i<isPrime.Length;i++) isPrime[i] = true;
		for(int i=2;i<isPrime.Length;i++){
			if(!isPrime[i]) continue;
			Primes.Add(i);
			for(int j=i+i;j<isPrime.Length;j+=i){
				isPrime[j] = false;
			}
		}
		//Console.WriteLine(Primes.Count);
	}
	
	public Dictionary<BI, int> Factorize(BI x){
		if(x == 1) return new Dictionary<BI, int>();
		if(x < TrialDivLimit) return FactorizeByTrial(x);
		if(x < SieveDivLimit) return FactorizeBySieve(x);
		return ProbalbityFactorize(x);
	}
	
	public Dictionary<BI, int> FactorizeByTrial(BI x){
		var ret = new Dictionary<BI, int>();
		for(BI p=2;p*p<=x;p++){
			if(x % p != 0) continue;
			ret.Add(p, 0);
			while(x % p == 0){
				ret[p]++;
				x /= p;
			}
		}
		if(x != 1) ret.Add(x, 1);
		return ret;
	}
	public Dictionary<BI, int> FactorizeBySieve(BI x){
		var ret = new Dictionary<BI, int>();
		foreach(var p in Primes){
			if(x % p != 0) continue;
			ret.Add(p, 0);
			while(x % p == 0){
				ret[p]++;
				x /= p;
			}
		}
		if(x != 1) ret.Add(x, 1);
		return ret;
	}
	
	public Dictionary<BI, int> ProbalbityFactorize(BI x){
		var ret = new Dictionary<BI, int>();
		foreach(var p in Primes){
			if(x % p != 0) continue;
			ret.Add(p, 0);
			while(x % p == 0){
				ret[p]++;
				x /= p;
			}
		}
		
		if(x == 1) return ret;
		
		BI lb = 0;
		if(MyMath.IsSquare(x, ref lb)){
			ret.Add(lb, 2);
			x /= lb;
			x /= lb;
			return ret;
		}
		
		if(MyMath.MillerRabinTest(x)){
			ret.Add(x, 1);
			return ret;
		}
		
		BI m1 = MyMath.Rho(x);
		BI m2 = x / m1;
		ret.Add(m1,1);
		if(m2 != 1) ret.Add(m2,1);
		
		return ret;
	}
}

class MyMath{
	
	public static bool MillerRabinTest(BI n){
		int rep = 200;
		BI d = n - 1;
		// (n-1)=2^s*d
		while((d & 1) == 0) d >>= 1;
		
		var rnd = new Xor128(2525);
		BI a = 1 + ((BI) rnd.Next()) % (n - 2);
		
		while(rep > 0){
			rep--;
			a = 1 + ((BI)rnd.Next() * a) % (n - 2);
			BI t = d;
			BI y = ModPow(a, t, n);
			while(t != n - 1 && y != 1 && y != n - 1){
				y = y * y % n;
				t <<= 1;
			}
			if(y != (n - 1) && (t & 1) == 0)return false;
		}
		return true;
	}
	
	public static BI Rho(BI n){
		// find a prime factor
		// https://ja.wikipedia.org/wiki/%E3%83%9D%E3%83%A9%E3%83%BC%E3%83%89%E3%83%BB%E3%83%AD%E3%83%BC%E7%B4%A0%E5%9B%A0%E6%95%B0%E5%88%86%E8%A7%A3%E6%B3%95
		BI[] seeds = new BI[]{3,5,7,11,13,17,19};
		BI ret = n;
		foreach(var seed in seeds){
			Func<BI, BI> f = x => (x * x + seed) % n;
			
			BI x = 2, y = 2, d = 1;
			while(d == 1){
				x = f(x);
				y = f(f(y));
				d = GCD(Abs(x - y), n);
				//Console.WriteLine("{0} {1} {2}", x, y, d);
			}
			BI result = d;
			if(d < n) return d;
		}
		return n;
	}
	
	public static BI GCD(BI a, BI b){
		return a == 0 ? b : GCD(b % a, a);
	}
	public static BI Abs(BI x){
		return x < 0 ? -x : x;
	}
	
	public static BI ModPow(BI a, BI t, BI mod){
		BI ret = 1;
		while(t > 0){
			if((t & 1) == 1){ret *= a; ret %= mod;};
			a = a * a; a %= mod;
			t>>=1;
		}
		return ret;
	}
	
	public static bool IsSquare(BI x, ref BI lb){
		BI l = 0, r = (BI) 2e9;
		while(r - l > 1){
			var c = (l + r) / 2;
			if(c * c <= x){
				l = c;
			} else {
				r = c;
			}
		}
		lb = l;
		return l * l == x;
	}
	
}

class Xor128{
	uint x = 123456789;
	uint y = 362436069;
	uint z = 521288629;
	uint w = 88675123; 
	uint t;
	
	public Xor128(){
	}
	
	public Xor128(uint seed){
		z ^= seed;
		z ^= z >> 21; z ^= z << 35; z ^= z >> 4;
	}
	
	
	public uint Next(){
		t = x ^ (x << 11);
		x = y; y = z; z = w;
		return w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
	}
	public int Next(int ul){
		return NextI(0, ul-1);
	}
	public int NextI(int from, int to){
		int mod = to - from + 1;
		int ret = (int)(Next() % mod);
		return ret + from;
	}
}

class Test{
	static void Main(){
		
		BI[] testcases = new BI[]{
			BI.Parse("999381247093216751"),
			BI.Parse("1000000016000000063"),
			BI.Parse("2305843009213693951"),
			BI.Parse("2305843009213693952"),
			1,
			
		};
		
		var sw = new Stopwatch();
		foreach(BI x in testcases){
			sw.Start();
			var t0 = sw.ElapsedMilliseconds;
			var fc = new Factorization();
			var t1 = sw.ElapsedMilliseconds;
			var res = fc.Factorize(x);
			var t2 = sw.ElapsedMilliseconds;
			
			Console.WriteLine("testcase: {0} :", x);
			Console.WriteLine(" instantiation: {0}[ms] :", t1 - t0);
			Console.WriteLine(" factorization: {0}[ms] :", t2 - t1);
			Console.WriteLine(" result: {0}", resultString(x, res));
			Console.WriteLine();
		}
	}
	
	static String resultString(BI x, Dictionary<BI, int> di){
		var l = new List<BI>();
		foreach(var k in di.Keys) for(int i=0;i<di[k];i++) l.Add(k);
		l.Sort();
		return String.Format("{0} = {1}", x, l.Count == 0 ? "1" : String.Join(" * ", l));
	}
}

/*
testcase: 999381247093216751 :
 instantiation: 54[ms] :
 factorization: 49[ms] :
 result: 999381247093216751 = 999665081 * 999716071

testcase: 1000000016000000063 :
 instantiation: 50[ms] :
 factorization: 79[ms] :
 result: 1000000016000000063 = 1000000007 * 1000000009

testcase: 2305843009213693951 :
 instantiation: 50[ms] :
 factorization: 20[ms] :
 result: 2305843009213693951 = 2305843009213693951

testcase: 2305843009213693952 :
 instantiation: 52[ms] :
 factorization: 9[ms] :
 result: 2305843009213693952 = 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2
*/	

