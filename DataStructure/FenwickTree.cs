using System;

class FenwickTree {
	// field
	long[] Data;
	public int N;		// base space size 
	int n;				// total size (pow of 2)
	
	// constructor
	public FenwickTree(int n_){
		N = n_;
		n = 1 << 2;
		while(n < N * 2) n <<= 1;
		Data = new long[n + 1];
	}

	// add at [0, i)
	public void Add(int i, long x){
		while(i <= n){
			Data[i] += x;
			i += (i & -i);
		}
	}
	
	// i => sum(a_k) for k in [0, i)
	public long Cumulate(int i){
		long s = 0;
		while(i > 0){
			s += Data[i];
			i -= (i & -i);
		}
		return s;
	}
}

class FenwickTree<T> {
	// field
	T[] Data;
	public int N;		// base space size 
	int n;				// total size (pow of 2)
	T Zero;
	Func<T, T, T> Op;
	
	// constructor
	public FenwickTree(int n_, Func<T, T, T> op_, T zero_){
		N = n_;
		n = 1 << 2;
		while(n < N * 2) n <<= 1;
		Zero = zero_;
		Op = op_;
		Data = new T[n + 1];
		for(int i = 0; i < n + 1; i++) Data[i] = Zero;
	}

	// add at [0, i)
	public void Add(int i, T x){
		while(i <= n){
			Data[i] = Op(Data[i], x);
			i += (i & -i);
		}
	}
	
	// i => sum(a_k) for k in [0, i)
	public T Cumulate(int i){
		T s = Zero;
		while(i > 0){
			s = Op(s, Data[i]);
			i -= (i & -i);
		}
		return s;
	}
}


class Test{
	
	static void Main(){
		
		int N = 10;
		var rnd = new Random(2525);
		
		var BT = new FenwickTree(N);
		int T1 = 100;
		long[] a = new long[N];
		
		for(int t1=0;t1<T1;t1++){
			
			var idx = rnd.Next(N);
			var v = rnd.Next(123456);
			a[idx] += v;
			BT.Add(idx + 1, v);
			
			for(int j=0;j<=N;j++){
				long bitsum = BT.Cumulate(j);
				long naivesum = 0;
				for(int k=0;k<j;k++){
					naivesum += a[k];
				}
				
				if(naivesum != bitsum){
					Console.WriteLine("naive:{0}, bit:{1}, error",  naivesum, bitsum);
				}
			}
		}
		Console.WriteLine("ok");
		
		var BT2 = new FenwickTree<double>(N, (x, y) => x + y, 0.0);
		double[] b = new double[N];
		double eps = 1e-9;
		
		for(int t1=0;t1<T1;t1++){
			
			var idx = rnd.Next(N);
			double v = rnd.Next(123456);
			double v2 = rnd.Next(1, 20);
			v /= v2;
			b[idx] += v;
			BT2.Add(idx + 1, v);
			
			for(int j=0;j<=N;j++){
				double bitsum = BT2.Cumulate(j);
				double naivesum = 0;
				for(int k=0;k<j;k++){
					naivesum += b[k];
				}
				
				if(Math.Abs(naivesum - bitsum) > eps){
					Console.WriteLine("naive:{0}, bit:{1}, error",  naivesum, bitsum);
				}
			}
		}
		Console.WriteLine("ok");
		
	}
	
	
}