#pragma once

#include "pch.h"
using namespace DirectX;

class Ball
{
public:
	Ball() 
	{
		position = XMFLOAT2(0,0);
		velocity = XMFLOAT2(0,0);
	};

	XMFLOAT2 position;
	XMFLOAT2 velocity;
};
