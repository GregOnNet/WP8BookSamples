#pragma once
#include "PlayerBase.h"

class Playground;
class AiPlayer : public PlayerBase
{
public:
	Playground* Parent;

public:
	AiPlayer(Playground* parent)
	{
		Parent = parent;
		Score = 0;	
	};
	~AiPlayer(void);

	void Update(float elapsedTime) override
	{
		Intention = Parent->ball->position.x / Parent->screenWidth;

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
