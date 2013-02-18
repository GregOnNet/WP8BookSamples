#include "pch.h"
#include "Playground.h"
#include "Player.h"
#include "AiPlayer.h"

#include "DDSTextureLoader.h"

#include "Helpers.h"
#include "VertexTypes.h"

using namespace DirectX;

Playground::Playground(Microsoft::WRL::ComPtr<ID3D11Device1> device, 					  Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context)
{
	initialized = false;
	d3dDevice = device;
	d3dContext = context;
}

Playground::~Playground(void)
{

}

void Playground::Init(float screenWidth, float screenHeight)
{
	this->screenWidth = screenWidth;
	this->screenHeight = screenHeight;

	CreateDDSTextureFromFile(d3dDevice.Get(),L"ball.dds",NULL,ballTexture.GetAddressOf(),NULL);
	CreateDDSTextureFromFile(d3dDevice.Get(),L"pix.dds",NULL,pix.GetAddressOf(),NULL);
		
	uiFont = std::unique_ptr<SpriteFont>(new SpriteFont(d3dDevice.Get(),L"UIFont.spritefont"));

	// spieler erstellen
	topPlayer = new Player();
	topPlayer->MaxSpeed = MAX_PLAYER_SPEED;
	bottomPlayer = new Player();
	bottomPlayer->MaxSpeed = MAX_PLAYER_SPEED;

	// neuen Ball erstellen
	ball = new Ball();

	// Spielfeld zurücksetzen
	Reset();

	initialized = true;
}

void Playground::Reset()
{
	gameStarted = false;

	// Ball initialisieren
	ball->velocity = XMFLOAT2(BALL_SPEED_X,BALL_SPEED_Y);
	ball->position = XMFLOAT2(screenWidth / 2, screenHeight / 2);

	// spieler zentrieren
	topPlayer->Intention = 0.5f;
	topPlayer->Position = 0.5f;

	bottomPlayer->Intention = 0.5f;
	bottomPlayer->Position = 0.5f;

}

void Playground::Update(float timeTotal, float timeDelta)
{
	// Update der Spieler
	topPlayer->Update(timeDelta);
	bottomPlayer->Update(timeDelta);

	// Ballgeschwindigkeit in pixel umrechnen
	float ballSpeedX = ball->velocity.x * screenWidth * timeDelta;
	float ballSpeedY = ball->velocity.y * screenWidth * timeDelta;

	// Ball bewegen
	ball->position.x += ballSpeedX;
	ball->position.y += ballSpeedY;

	// Ball radius berechnen
	float ballRadius = (BALL_SIZE * screenWidth) / 2;

	// Kollision in x-Richtung
	if(ball->position.x < ballRadius)
	{
		ball->position.x = ballRadius;
		ball->velocity.x *= -1;
	}
	else if(ball->position.x > screenWidth - ballRadius)
	{
		ball->position.x = screenWidth - ballRadius;
		ball->velocity.x *= -1;
	}

	
	float playerHeight = PLAYER_HEIGHT * screenHeight;
	float playerWidth = PLAYER_WIDTH * screenWidth;

	float ballTop = ball->position.y  - ballRadius;
	float ballBottom = ball->position.y + ballRadius;

	// tor oben
	if(ballTop < playerHeight)
	{
		// Linke und rechte Spielerkante
		float pLeft = (topPlayer->Position * screenWidth) - (playerWidth / 2);
		float pRight = (topPlayer->Position * screenWidth) + (playerWidth / 2);

		if(ball->position.x > pLeft && ball->position.x < pRight)
		{
			// abgewehrt
			ball->position.y = playerHeight + ballRadius;
			ball->velocity.y *= -1;
		}
		else
		{
			// tor
			bottomPlayer->Score ++;
			Reset();
		}
	} 
	// tor unten
	else if(ball->position.y > screenHeight - ballRadius - playerHeight)
	{
		// Linke und rechte Spielerkante
		float pLeft = (bottomPlayer->Position * screenWidth) - (playerWidth / 2);
		float pRight = (bottomPlayer->Position * screenWidth) + (playerWidth / 2);
		
		if(ball->position.x > pLeft && ball->position.x < pRight)
		{
			// abgewehrt
			ball->position.y = screenHeight - ballRadius - playerHeight;
			ball->velocity.y *= -1;
		}
		else
		{
			// tor
			topPlayer->Score++;
			Reset();
		}
	}

	
	

}

void Playground::Render2D(SpriteBatch* batch)
{
	// Mittellinie zeichnen
	RECT line;
	line.left = 0;
	line.right = screenWidth;
	line.top = (screenHeight / 2) - 1;
	line.bottom = (screenHeight / 2) + 1;
    
	batch->Draw(pix.Get(),line,Colors::DarkRed);
    
	// größe der Spieler berechnen
	float playerWidth = PLAYER_WIDTH * screenWidth;
	float playerHeight = PLAYER_HEIGHT * screenHeight;
    
	// aktuelle Spieler Position in pixeln
	float topPlayerX = topPlayer->Position * screenWidth;
	float bottomPlayerX = bottomPlayer->Position * screenWidth;
    
	// rechteck für oberen Spieler
	RECT topPlayerRect;
	topPlayerRect.top = 0;
	topPlayerRect.bottom = playerHeight;
	topPlayerRect.left = topPlayerX - playerWidth / 2;
	topPlayerRect.right = topPlayerX + playerWidth / 2;
    
	// rechteck für unteren Spieler
	RECT bottomPlayerRect;
	bottomPlayerRect.top = screenHeight - playerHeight;
	bottomPlayerRect.bottom = screenHeight;
	bottomPlayerRect.left = bottomPlayerX - playerWidth / 2;
	bottomPlayerRect.right = bottomPlayerX + playerWidth / 2;
    
	// Spieler zeichnen
	batch->Draw(pix.Get(),topPlayerRect);
	batch->Draw(pix.Get(),bottomPlayerRect);
    

    wchar_t* topScore = StringFormat("%d",topPlayer->Score);
    wchar_t* botScore = StringFormat("%d",bottomPlayer->Score);
    
    // größe des Textes berechnen
    XMFLOAT2 topSize;
    XMStoreFloat2(&topSize,uiFont->MeasureString(topScore));
    XMFLOAT2 botSize;
    XMStoreFloat2(&botSize,uiFont->MeasureString(botScore));
    
    // punktestand zeichnen
    uiFont->DrawString(batch,topScore,XMFLOAT2(screenWidth / 2 - (topSize.x / 2),screenHeight * 0.25 - (topSize.y / 2)),Colors::LightGray);
    uiFont->DrawString(batch,botScore,XMFLOAT2(screenWidth / 2 - (botSize.x / 2),screenHeight * 0.75 - (botSize.y / 2)),Colors::LightGray);
    
    // strings wieder löschen
    delete topScore;
    delete botScore;


	// Ball radius berechnen
	float ballRadius = (BALL_SIZE * screenWidth) / 2;
       
	RECT ballRect;
	ballRect.left = ball->position.x - ballRadius;
	ballRect.right = ball->position.x + ballRadius;
	ballRect.top = ball->position.y  - ballRadius;
	ballRect.bottom = ball->position.y + ballRadius;
    
	// Ball zeichnen;
	batch->Draw(ballTexture.Get(),ballRect);
}


void Playground::HandleTouch(float x, float y)
{
	if(y >= screenHeight / 2)
	{
		bottomPlayer->Intention = x / screenWidth;
	}
	else
	{
		topPlayer->Intention = x / screenWidth;
	}
}
