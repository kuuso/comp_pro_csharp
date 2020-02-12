using System;

class SegTree<T>{
	// field
	T[] Data;
	T IdentityElement;
	Func<T, T, T> Op;
	public int N;		// base space size 
	int n;				// total size (pow of 2)
	
	//constructor
	public SegTree(int n_, Func<T, T, T> op_, T inf_){
		N = n_;
		IdentityElement = inf_;
		n = 1;
		while(n < n_) n *= 2;
		Op = op_;
		Data = new T[2 * n - 1];
		for(int i = 0; i < 2 * n - 1; i++) Data[i] = IdentityElement;
	}
	
	public void Update(int idx, T v){
		idx += n - 1;
		Data[idx] = v;
		while(idx > 0){
			idx = (idx - 1) / 2;
			Data[idx] = Op(Data[idx * 2 + 1], Data[idx * 2 + 2]);
		}
	}
	
	public T Query(int a, int b){
		// Op([a, b))
		return Query(a, b, 0, 0, n);
	}
	
	T Query(int a,int b,int k,int l,int r){
		if(r <= a || b <= l) return IdentityElement;
		if(a <= l && r <= b) return Data[k];
		T vl = Query(a, b, k * 2 + 1, l, (l + r) / 2);
		T vr = Query(a, b, k * 2 + 2, (l + r) / 2, r);
		return Op(vl, vr);
	}
	
	public T QueryAll{
		get{ return Data[0];}
	}
	
	public T At(int idx){
		return Data[idx + n - 1];
	}
	
	public void UniqInit(T v){
		for(int i = 0 + n - 1; i < N + n - 1; i++) Data[i] = v;
		for(int i = n - 2; i >= 0; i--){
			Data[i] = Op(Data[i * 2 + 1], Data[i * 2 + 2]);
		}
	}
	
	public void Init(T[] a){
		for(int i = 0; i < a.Length; i++) Data[i + n - 1] = a[i];
		for(int i = n - 2; i >= 0; i--){
			Data[i] = Op(Data[i * 2 + 1], Data[i * 2 + 2]);
		}
	}
	public void Dump(){
		Console.WriteLine();
		int h = 0;
		int cnt = 0;
		for(int i = 0; i < Data.Length; i++){
			Console.Write("{0} ", Data[i]);
			cnt++;
			if(cnt == 1 << h){
				cnt = 0;
				h++;
				Console.WriteLine();
			}
		}
	}
}

class Test{
	
	static void Main(){
		
		int N = 10;
		var rnd = new Random(2525);
		int[] a = new int[N];
		for(int i=0;i<N;i++) a[i] = rnd.Next(100);
		Console.WriteLine(String.Join(" ", a));
		
		var st = new SegTree<int>(N, Math.Max, (int)-1);
		st.Init(a);
		st.Dump();
		
		Console.WriteLine(st.Query(0, 5));
		Console.WriteLine(st.Query(7, 9));
		st.Update(3, 314159);
		st.Dump();
		Console.WriteLine(st.Query(0, 5));
		Console.WriteLine(st.Query(7, 9));
	}
	
	
}