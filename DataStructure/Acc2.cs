using System;
using System.Collections;
using System.Collections.Generic;


class Acc2{
	public int H, W;
	public long[][] acc;
	public long Sum(int x1, int y1, int x2, int y2){
		//return num of land [x1,x2)*[y1,y2); left-inclusive / right-noninclusive;
		if(x2 < x1 || y2 < y1) return 0;
		return acc[y2][x2] - acc[y2][x1] - acc[y1][x2] + acc[y1][x1];
	}
	
	public void Init(long[][] M){
		H = M.Length;
		W = M[0].Length;
		acc = new long[H+1][];
		for(int i = 0; i <= H; i++){
			acc[i] = new long[W + 1];
			if(i == 0) continue;
			for(int j = 1; j <= W; j++){
				acc[i][j] = M[i - 1][j - 1];
			}
		}
		
		for(int i = 1; i <= H; i++){
			for(int j = 1; j <= W; j++){
				acc[i][j] += acc[i][j-1];
			}
		}
		for(int j = 1; j <= W; j++){
			for(int i = 1; i <= H; i++){
				acc[i][j] += acc[i - 1][j];
			}
		}
	}
	
	public static long Naive(long[][] a, int x1, int y1, int x2, int y2){
		long ret = 0;
		for(int i = y1; i < y2; i++){
			for(int j = x1; j < x2; j++){
				ret += a[i][j];
			}
		}
		return ret;
	}
	
	
}

class TEST{
	static void Main(){
		
		int N = 10; 
		long[][] a = new long[N][];
		for(int i=0;i<N;i++){
			a[i] = new long[N];
			for(int j=0;j<N;j++){
				a[i][j] = i * N + j;
			}
		}
		
		var acc = new Acc2();
		acc.Init(a);

		var rnd = new Random(2525);
		for(int t = 0; t < 4; t++){
			int x1 = rnd.Next(N);
			int y1 = rnd.Next(N);
			int x2 = rnd.Next(x1, N + 1);
			int y2 = rnd.Next(y1, N + 1);
			long res = acc.Sum(x1, y1, x2, y2);
			long gt = Acc2.Naive(a, x1, y1, x2, y2);
			Console.WriteLine("[{0}, {1}) * [{2}, {3}): res:{4}, gt:{5}, assert:{6}",
				x1, x2, y1, y2, res, gt, res == gt
			);
		}
		
	}
}