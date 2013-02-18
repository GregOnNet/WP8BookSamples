#pragma once
#include "PlayerBase.h"

class Player : public PlayerBase
{
public:
	Player(void){ Score = 0;};
	~Player(void) {};

	void Update(float elapsedTime) override
	{
		float limit = MaxSpeed * elapsedTime;

		float diff = abs(Intention - Position);
		if(Intention < Position)
		{
			Position -= min(diff,limit);
		}
		else
		{
			Position += min(diff,limit);
		}
	};
};
