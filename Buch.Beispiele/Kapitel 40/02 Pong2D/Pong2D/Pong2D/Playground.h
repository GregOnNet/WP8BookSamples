#pragma once
#include "SpriteBatch.h"
#include "SpriteFont.h"
#include "Direct3DBase.h"

#include "Ball.h"
#include "PlayerBase.h"
#include "Player.h"

#include "Effects.h"

using namespace DirectX;

#define BALL_SIZE 0.1f
#define BALL_SPEED_X 0.25f
#define BALL_SPEED_Y 0.7f
#define PLAYER_HEIGHT 0.02f
#define PLAYER_WIDTH 0.2f
#define MAX_PLAYER_SPEED 0.8f


class Playground
{
public:
	float screenWidth;
	float screenHeight;

	Ball* ball;
	PlayerBase* topPlayer;
	PlayerBase* bottomPlayer;

private:
	Microsoft::WRL::ComPtr<ID3D11Device1> d3dDevice;
	Microsoft::WRL::ComPtr<ID3D11DeviceContext1> d3dContext;
	bool gameStarted;
	bool initialized;
	int bumpOffset;
	
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> ballTexture;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> background;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> playerTexture;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> pix;

	std::unique_ptr<DirectX::SpriteFont> uiFont;

public:
	Playground(Microsoft::WRL::ComPtr<ID3D11Device1>device, Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context);
	~Playground(void);

	void Init(float screenWidth, float screenHeight);
	void Reset();
	void Update(float timeTotal, float timeDelta);
	void Render2D(SpriteBatch* batch);

	void HandleTouch(float x, float y);
};
