#pragma once

class PlayerBase
{
public:
	float Position;
	float Intention;
	int Score;
	
	float MaxSpeed;

public:
	PlayerBase(void) {};
	~PlayerBase(void) {};

	virtual void Update(float elapsedTime) {};
};
