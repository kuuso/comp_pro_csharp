using System;

class UnionFind{
	
	int[] parent;
	int[] mem;
	int compo;
	int N;
	public UnionFind(int n_){
		N = n_;
		parent = new int[N];
		mem = new int[N];
		for(int i = 0; i < N; i++){
			parent[i] = i;
			mem[i] = 1;
		}
		compo = N;
	}
	
	public int Parent(int a){
		if(parent[a] == a) return a;
		return parent[a] = Parent(parent[a]);
	}
	
	public bool United(int a, int b){
		return Parent(a) == Parent(b);
	}
	
	public bool Unite(int a, int b){
		a = Parent(a); b = Parent(b);
		if(a == b) return false;
		if(mem[a] > mem[b]){var t = a; a = b; b = t;}
		parent[a] = b;
		mem[b] += mem[a];
		compo -= 1;
		return true;
	}
	
	public bool IsRoot(int x){
		return x == parent[x];
	}
	public int MemCnt(int x){
		return mem[Parent(x)];
	}
	
	public void Dump(){
		Console.WriteLine(String.Join(" ", parent));
	}
	
	public int Compo{
		get{
			return compo;
		}
	}
	
}

class Test{
	static void Main(){
		int N = 10;
		var UF = new UnionFind(N);
		Console.WriteLine("{0} and {1} unite:{2}, compo:{3}", 1, 3, UF.Unite(1, 3), UF.Compo);
		Console.WriteLine("{0} and {1} unite:{2}, compo:{3}", 4, 2, UF.Unite(4, 2), UF.Compo);
		Console.WriteLine("{0} and {1} unite:{2}, compo:{3}", 6, 4, UF.Unite(6, 4), UF.Compo);
		Console.WriteLine("{0} and {1} unite:{2}, compo:{3}", 2, 4, UF.Unite(2, 4), UF.Compo);
	}
}