using System;
using System.Collections;
using System.Collections.Generic;


// Use 2 or more instance to avoid hash collision. 
//   Better not use tuple or xor hashing in that case. Naive AND recommended.
class RollingHash {
	
	long[] Hash;
	long[] Base;
	String S;
	long mod;
	int N;
	
	public RollingHash(String s_, long base_=2525, long mod_ = (long)1e9+7){
		mod = mod_;
		S = s_;
		N = S.Length;
		Hash = new long[N+1];
		
		Base = new long[N+1];
		Base[0] = 1;
		Base[1] = base_;
	
		for(int i=1;i<N;i++){
			Base[i+1] = Base[i] * Base[1];
			if(Base[i+1] >= mod) Base[i+1] %= mod;
		}
		
		Hash[0] = 0;
		Hash[1] = (long) S[0];
		for(int i = 1; i < N; i++){
			Hash[i + 1] = Hash[i] * Base[1] + (long) S[i];
			if(Hash[i + 1] >= mod) Hash[i + 1] %= mod;
		}
	}
	
	public long GetHash(int strt, int len){
		//returns hash of S.Substring(strt,len);
		if(len == 0) return 0;
		if(strt + len >= N) len = N - strt;
		long ret = Hash[strt + len] + mod - (Hash[strt] * Base[len] >= mod ? Hash[strt] * Base[len] % mod : Hash[strt] * Base[len]);
		return ret >= mod ? ret % mod : ret;
	}
	
}


class TEST{
	static void Main(){
		
		N = 100;
		var cs = new char[N];
		var rnd = new Random();
		for(int i=0;i<N;i++) cs[i] = (char) rnd.Next((int) 'a', (int) 'z' + 1);
		String S = new String(cs);
		int N = S.Length;
		int cnt = 0;
		int ff = 0;
		int tt = 0;
		int tf = 0;
		int ft = 0;
		int excep = 0;
		
		Console.WriteLine(S);
		var RH=new RollingHash(S);
		
		for(int i1=0;i1<N;i1++){
			for(int i2=0;i2<N;i2++){
				for(int l1=0;l1+i1<N;l1++){
					for(int l2=0;l2+i2<N;l2++){
						try{
							var s1 = S.Substring(i1,l1);
							var s2 = S.Substring(i2,l2);
							var Naive = s1 == s2;
							var Hashed = RH.GetHash(i1,l1) == RH.GetHash(i2,l2);
							var judge = Naive == Hashed;
							if(!judge){
								Console.WriteLine("S={0}",S);
								Console.WriteLine("\ti1 = {0}, l1 = {1}, Substring = {2}", i1, l1, s1);
								Console.WriteLine("\ti2 = {0}, l2 = {1}, Substring = {2}", i2, l2, s2);
								Console.WriteLine("\t\tNaive = {0}", Naive);
								Console.WriteLine("\ti1 = {0}, l1 = {1}, Hash = {2}", i1, l1, RH.GetHash(i1,l1));
								Console.WriteLine("\ti2 = {0}, l2 = {1}, Hash = {2}", i2, l2, RH.GetHash(i2,l2));
								if(!Naive && Hashed) ft++;
								if(Naive && !Hashed) tf++;
								return;
							} else {
								if(Naive && Hashed) tt++;
								if(!Naive && !Hashed) ff++;
							}
							cnt++;
						}catch{
							excep++;
						}
					}
				}
			}
		}
		
		Console.WriteLine("All {0} case\nPass(total: {3}): tt = {1}, ff = {2}\nFail(total: {6}): ft = {4}, tf = {5}",cnt,tt,ff,tt + ff,ft,tf,ft + tf);
		Console.WriteLine("Exception:{0}",excep);
	}
}
