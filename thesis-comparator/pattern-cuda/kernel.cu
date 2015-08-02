
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>
#include <conio.h>


__global__ void kernel(char *dbText, char *pattern, char *result, int patternLength)
{
	int i = blockDim.x * blockIdx.x + threadIdx.x;

	bool flag = false;
	for (int j = 0; j < patternLength; j++){
		if (dbText[i + j] != pattern[j]){
			flag = true;
			break;
		}
	}
	if (flag){
		result[i] = 0;
	}
	else{
		result[i] = 1;
	}
}

int main(){
	return 0;
}
