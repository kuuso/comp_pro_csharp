using System;
using System.Collections;
using System.Collections.Generic;

static class Rot{
	// [0,x) x [0,y) @ manhattan -> [0,xx) x [0, yy) chevichef
	// XM,YM returns -1 if cannot convert into integer
	public const int L0 = 1000;
	public static int XC(int xm, int ym, int L = L0){
		return xm + ym;
	}
	public static int YC(int xm, int ym, int L = L0){
		return -xm + ym + L;
	}
	public static int XM(int xc, int yc, int L = L0){
		return (xc - yc + L) % 2 != 0 ? -1 : (xc - yc + L) / 2;
	}
	public static int YM(int xc, int yc, int L = L0){
		return (xc + yc - L) % 2 != 0 ? -1 : (xc + yc - L) / 2;
	}
	
}


class Test{
	static void Main(){
		int N = 5;
		int L = N;
		int[][] a = new int[N][];
		for(int i=0;i<N;i++){
			a[i] = new int[N];
			for(int j=0;j<N;j++) a[i][j] = i * N + j;
		}
		
		int[][] b = new int[2 * N][];
		for(int i=0;i<b.Length;i++){
			b[i] = new int[2 * N];
			for(int j=0;j<b[i].Length;j++){
				b[i][j] = -1;
			}
		}
		for(int y=0;y<N;y++){
			for(int x=0;x<N;x++){
				var nx = Rot.XC(x, y, L);
				var ny = Rot.YC(x, y, L);
				b[ny][nx] = a[y][x];
			}
		}
		
		for(int i=0;i<b.Length;i++) Console.WriteLine(String.Join(" ",b[i]));
		
		int[][] c = new int[N][];
		for(int i=0;i<N;i++){
			c[i] = new int[N];
			for(int j=0;j<N;j++) c[i][j] = -1;
		}
		
		for(int y=0;y<b.Length;y++){
			for(int x=0;x<b[y].Length;x++){
				if(b[y][x] == -1) continue;
				var nx = Rot.XM(x, y, L);
				var ny = Rot.YM(x, y, L);
				c[ny][nx] = b[y][x];
			}
		}
		for(int i=0;i<c.Length;i++) Console.WriteLine(String.Join(" ",c[i]));
		
	}
}

