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

	basicEffect = std::unique_ptr<BasicEffect>(new BasicEffect(d3dDevice.Get()));
	basicEffect->SetTextureEnabled(true);
	basicEffect->EnableDefaultLighting();

	float texLeft = 480.0f/1024.0f;
	float texBottom = 800.0f/1024.0f;
	XMFLOAT3 normalUp(0,1,0);

	// Spielfeld erstellen
	VertexPositionNormalTexture planeVertices[6];

	// oben links, oben rechts, unten links
	planeVertices[0] = VertexPositionNormalTexture(XMFLOAT3(0,0,0), normalUp, XMFLOAT2(0,0)); 
	planeVertices[1] = VertexPositionNormalTexture(XMFLOAT3(4.8,0,0), normalUp, XMFLOAT2(texLeft,0));
	planeVertices[2] = VertexPositionNormalTexture(XMFLOAT3(0,0,8), normalUp, XMFLOAT2(0,texBottom));

	// oben rechts, unten recht, unten links
	planeVertices[3] = VertexPositionNormalTexture(XMFLOAT3(4.8,0,0), normalUp, XMFLOAT2(texLeft,0));
	planeVertices[4] = VertexPositionNormalTexture(XMFLOAT3(4.8,0,8), normalUp, XMFLOAT2(texLeft,texBottom));
	planeVertices[5] = VertexPositionNormalTexture(XMFLOAT3(0,0,8), normalUp, XMFLOAT2(0,texBottom));

	D3D11_SUBRESOURCE_DATA vertexBufferData;
	vertexBufferData.pSysMem = planeVertices;
	vertexBufferData.SysMemPitch = 0;
	vertexBufferData.SysMemSlicePitch = 0;
	CD3D11_BUFFER_DESC vertexBufferDesc(sizeof(planeVertices), D3D11_BIND_VERTEX_BUFFER);
	d3dDevice->CreateBuffer(&vertexBufferDesc, &vertexBufferData, planeVertexBuffer.GetAddressOf());

	XMFLOAT3 normUp(0,1,0);
	XMFLOAT3 normFront(0,0,1);
	XMFLOAT3 normLeft(-1,0,0);
	XMFLOAT3 normRight(1,0,0);

	// Spieler erstellen
	VertexPositionNormalTexture cubeVertices[16];

	// top
	cubeVertices[0] = VertexPositionNormalTexture(XMFLOAT3(-0.5f,0.5f,-0.5f),normUp,XMFLOAT2(0,0.5));
	cubeVertices[1] = VertexPositionNormalTexture(XMFLOAT3( 0.5f,0.5f,-0.5f),normUp,XMFLOAT2(1,0.5));
	cubeVertices[2] = VertexPositionNormalTexture(XMFLOAT3(-0.5f,0.5f, 0.5f),normUp,XMFLOAT2(0,0.9));
	cubeVertices[3] = VertexPositionNormalTexture(XMFLOAT3( 0.5f,0.5f, 0.5f),normUp,XMFLOAT2(1,0.9));
	
	// front
	cubeVertices[4] = VertexPositionNormalTexture(XMFLOAT3(-0.5f, 0.5f, 0.5f),normFront,XMFLOAT2(0,0.9));
	cubeVertices[5] = VertexPositionNormalTexture(XMFLOAT3( 0.5f, 0.5f, 0.5f),normFront,XMFLOAT2(1,0.9));
	cubeVertices[6] = VertexPositionNormalTexture(XMFLOAT3(-0.5f,-0.5f, 0.5f),normFront,XMFLOAT2(0,0.5));
	cubeVertices[7] = VertexPositionNormalTexture(XMFLOAT3( 0.5f,-0.5f, 0.5f),normFront,XMFLOAT2(1,0.5));

	// left
	cubeVertices[8] = VertexPositionNormalTexture(XMFLOAT3(-0.5f, 0.5f, -0.5f),normLeft,XMFLOAT2(0,0.5));
	cubeVertices[9] = VertexPositionNormalTexture(XMFLOAT3(-0.5f, 0.5f,  0.5f),normLeft,XMFLOAT2(0.5,0.5));
	cubeVertices[10] = VertexPositionNormalTexture(XMFLOAT3(-0.5f,-0.5f, -0.5f),normLeft,XMFLOAT2(0,0.9));
	cubeVertices[11] = VertexPositionNormalTexture(XMFLOAT3(-0.5f,-0.5f,  0.5f),normLeft,XMFLOAT2(0.5,0.9));

	// right
	cubeVertices[12] = VertexPositionNormalTexture(XMFLOAT3(0.5f, 0.5f, -0.5f),normRight,XMFLOAT2(0.5,0.9));
	cubeVertices[13] = VertexPositionNormalTexture(XMFLOAT3(0.5f, 0.5f,  0.5f),normRight,XMFLOAT2(1,0.9));
	cubeVertices[14] = VertexPositionNormalTexture(XMFLOAT3(0.5f,-0.5f, -0.5f),normRight,XMFLOAT2(0.5,0.5));
	cubeVertices[15] = VertexPositionNormalTexture(XMFLOAT3(0.5f,-0.5f,  0.5f),normRight,XMFLOAT2(1,0.5));

	unsigned short cubeIndices[] = 
	{
		// top
		0,1,2,
		1,3,2,

		//front
		4,5,6,
		5,7,6,

		//left
		8,9,10,
		9,11,10,

		//right
		12,14,13,
		13,14,15,
	};

	vertexBufferData.pSysMem = cubeVertices;
	vertexBufferData.SysMemPitch = 0;
	vertexBufferData.SysMemSlicePitch = 0;
	vertexBufferDesc = CD3D11_BUFFER_DESC(sizeof(cubeVertices), D3D11_BIND_VERTEX_BUFFER);
	d3dDevice->CreateBuffer(&vertexBufferDesc, &vertexBufferData, cubeVertexBuffer.GetAddressOf());

	D3D11_SUBRESOURCE_DATA indexBufferData;
	indexBufferData.pSysMem = cubeIndices;
	indexBufferData.SysMemPitch = 0;
	indexBufferData.SysMemSlicePitch = 0;
	CD3D11_BUFFER_DESC indexBufferDesc = CD3D11_BUFFER_DESC(sizeof(cubeIndices), D3D11_BIND_INDEX_BUFFER);
	d3dDevice->CreateBuffer(&indexBufferDesc, &indexBufferData,cubeIndexBuffer.GetAddressOf());

	// Ball erstellen
	VertexPositionNormalTexture ballVertices[6];
	ballVertices[0] = VertexPositionNormalTexture(XMFLOAT3(-0.5,0.5,0),normFront,XMFLOAT2(0,0));
	ballVertices[1] = VertexPositionNormalTexture(XMFLOAT3(0.5,0.5,0),normFront,XMFLOAT2(1,0));
	ballVertices[2] = VertexPositionNormalTexture(XMFLOAT3(-0.5,-0.5,0),normFront,XMFLOAT2(0,1));

	ballVertices[3] = VertexPositionNormalTexture(XMFLOAT3(-0.5,-0.5,0),normFront,XMFLOAT2(0,1));
	ballVertices[4] = VertexPositionNormalTexture(XMFLOAT3(0.5,0.5,0),normFront,XMFLOAT2(1,0));
	ballVertices[5] = VertexPositionNormalTexture(XMFLOAT3(0.5,-0.5,0),normFront,XMFLOAT2(1,1));
	
	vertexBufferData.pSysMem = ballVertices;
	vertexBufferData.SysMemPitch = 0;
	vertexBufferData.SysMemSlicePitch = 0;
	vertexBufferDesc = CD3D11_BUFFER_DESC(sizeof(ballVertices), D3D11_BIND_VERTEX_BUFFER);
	d3dDevice->CreateBuffer(&vertexBufferDesc,	&vertexBufferData, ballVertexBuffer.GetAddressOf());


	XMFLOAT3 eyepos(0,3,5.5);
	XMFLOAT3 lookat(0,-1,0);
	XMFLOAT3 up(0,1,0);
	view = XMMatrixLookAtRH(XMLoadFloat3(&eyepos),XMLoadFloat3(&lookat),XMLoadFloat3(&up));
	basicEffect->SetView(view);

	
	
	XMMATRIX projection =  XMMatrixRotationZ(-XM_PI / 2) * XMMatrixPerspectiveFovRH(XM_PIDIV2,(float)screenWidth / (float)screenHeight,0.01f,1000.0f);
	basicEffect->SetProjection(projection);

	void const* shaderByteCode;
	size_t byteCodeLength;
	basicEffect->GetVertexShaderBytecode(&shaderByteCode, &byteCodeLength);
	d3dDevice->CreateInputLayout(VertexPositionNormalTexture::InputElements, VertexPositionNormalTexture::InputElementCount, shaderByteCode, byteCodeLength, inputLayout.GetAddressOf());

	alphaEffect = std::unique_ptr<AlphaTestEffect>(new AlphaTestEffect(d3dDevice.Get()));
	alphaEffect->SetProjection(projection);
	alphaEffect->SetView(view);
	alphaEffect->SetReferenceAlpha(1);

	alphaEffect->GetVertexShaderBytecode(&shaderByteCode,&byteCodeLength);
	d3dDevice->CreateInputLayout(VertexPositionNormalTexture::InputElements,
						VertexPositionNormalTexture::InputElementCount,
							shaderByteCode, byteCodeLength,
							alphaInputLayout.GetAddressOf());

	CreateDDSTextureFromFile(d3dDevice.Get(),L"chrome.dds",NULL,ballTexture.GetAddressOf(),NULL);
	CreateDDSTextureFromFile(d3dDevice.Get(),L"pix.dds",NULL,pix.GetAddressOf(),NULL);
	CreateDDSTextureFromFile(d3dDevice.Get(),L"background.dds",NULL,background.GetAddressOf(),NULL);
	CreateDDSTextureFromFile(d3dDevice.Get(),L"player.dds",NULL,playerTexture.GetAddressOf(),NULL);
	
	uiFont = std::unique_ptr<SpriteFont>(new SpriteFont(d3dDevice.Get(),L"UIFont.spritefont"));

	// spieler erstellen
	topPlayer = new AiPlayer(this);
	topPlayer->MaxSpeed = MAX_PLAYER_SPEED;
	bottomPlayer = new Player();
	//bottomPlayer = new AiPlayer(this);
	bottomPlayer->MaxSpeed = MAX_PLAYER_SPEED;

	// neuen Ball erstellen
	ball = new Ball();

	// Spielfeld zurücksetzen
	Reset();

	initialized = true;
	bumpOffset = 0;
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

	bumpOffset = (int)(timeTotal * 10) * 2;
	bumpOffset %= 40;
	

}

void Playground::Render3D(SpriteBatch* batch)
{
	XMMATRIX worldMatrix = XMMatrixTranslation(-2.4f, 0, -4.0f);

	basicEffect->SetWorld(worldMatrix);
	basicEffect->SetTexture(background.Get());

	// effekt aktivieren
	basicEffect->Apply(d3dContext.Get());

	UINT stride = sizeof(VertexPositionNormalTexture);
	UINT offset = 0;

	d3dContext->IASetInputLayout(inputLayout.Get());
	d3dContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	d3dContext->IASetVertexBuffers(0,1,planeVertexBuffer.GetAddressOf(),&stride,&offset);

	// zeichnen
	d3dContext->Draw(6,0);

	// zu verwendende textur festlegen
	basicEffect->SetTexture(playerTexture.Get());

	// zu verwendende buffer festlegen
	d3dContext->IASetVertexBuffers(0,1,cubeVertexBuffer.GetAddressOf(),&stride,&offset);
	d3dContext->IASetIndexBuffer(cubeIndexBuffer.Get(),DXGI_FORMAT_R16_UINT,0);

	// spieler oben Transformationsmatrix berechnen
	XMMATRIX topPlayerWorld = XMMatrixScaling(4.8 * PLAYER_WIDTH, 0.3,0.3) * worldMatrix * XMMatrixTranslation(4.8 * topPlayer->Position,0.15,0.15);

	// effekt konfigurieren
	basicEffect->SetWorld(topPlayerWorld);
	basicEffect->Apply(d3dContext.Get());
	// zeichnen
	d3dContext->DrawIndexed(24,0,0);


	// Position des balls berechnen
	float ballX = -2.4f + (ball->position.x / screenWidth) * 4.8;
	float ballY = 2.4 * BALL_SIZE;
	float ballZ = -4 + (ball->position.y / screenHeight) * 8;

	// billboard matrix berechnen
	XMFLOAT4 r0,r1,r2,r3; 
	XMMATRIX billboardMatrix = XMMatrixInverse(NULL,view);
	billboardMatrix = XMMatrixMultiply(XMMatrixScaling(4.8 * BALL_SIZE, 4.8 * BALL_SIZE, 1), billboardMatrix);

	XMStoreFloat4(&r0, billboardMatrix.r[0]);
	XMStoreFloat4(&r1, billboardMatrix.r[1]);
	XMStoreFloat4(&r2, billboardMatrix.r[2]);
	XMStoreFloat4(&r3, billboardMatrix.r[3]);

	billboardMatrix = XMMATRIX(r0.x,r0.y,r0.z,r0.w,
								r1.x,r1.y,r1.z,r1.w,
								r2.x,r2.y,r2.z,r2.w,
								ballX,ballY,ballZ,r3.w);

	// effekt konfigurieren
	alphaEffect->SetWorld(billboardMatrix);
	alphaEffect->SetTexture(ballTexture.Get());
	alphaEffect->Apply(d3dContext.Get());

	
	d3dContext->IASetVertexBuffers(0,1,ballVertexBuffer.GetAddressOf(),&stride,&offset);
	d3dContext->IASetInputLayout(alphaInputLayout.Get());

	// ball zeichnen
	d3dContext->Draw(6,0);


	// zu verwendende textur festlegen
	basicEffect->SetTexture(playerTexture.Get());

	// zu verwendende buffer festlegen
	d3dContext->IASetVertexBuffers(0,1,cubeVertexBuffer.GetAddressOf(),&stride,&offset);
	d3dContext->IASetIndexBuffer(cubeIndexBuffer.Get(),DXGI_FORMAT_R16_UINT,0);
	d3dContext->IASetInputLayout(inputLayout.Get());

	// spieler unten Transformationmatrix berechnen
	XMMATRIX bottomPlayerWorld = XMMatrixScaling(4.8 * PLAYER_WIDTH, 0.3,0.3) * worldMatrix * XMMatrixTranslation(4.8 * bottomPlayer->Position,0.15,7.85);

	// effekt konfigurieren
	basicEffect->SetWorld(bottomPlayerWorld);
	basicEffect->Apply(d3dContext.Get());
	// zeichnen
	d3dContext->DrawIndexed(24,0,0);

	

	// zeichenkette erstellen
	wchar_t* score = StringFormat("%d:%d",topPlayer->Score, bottomPlayer->Score);

	// größe des Textes berechnen
	XMFLOAT2 scoreSize;
	XMStoreFloat2(&scoreSize,uiFont->MeasureString(score));

	// punktestand zeichnen
	uiFont->DrawString(batch,score,XMFLOAT2(screenWidth - scoreSize.y / 2,scoreSize.x / 2),Colors::LightGray,XM_PI / 2,XMFLOAT2(scoreSize.x / 2, scoreSize.y / 2));

	// zeichenkette wieder löschen
	delete score;
}

void Playground::Render2D(SpriteBatch* batch)
{
	//// Mittellinie zeichnen
	//RECT line;
	//line.left = 0;
	//line.right = screenWidth;
	//line.top = (screenHeight / 2) - 1;
	//line.bottom = (screenHeight / 2) + 1;

	//batch->Draw(pix.Get(),line,Colors::DarkRed);

	// Hintergrund zeichnen
	RECT bgSource;
	bgSource.top = 0;
	bgSource.left = 0;
	bgSource.right = 480;
	bgSource.bottom = 800;

	RECT backgroundRect;
	backgroundRect.top = 0;
	backgroundRect.left = 0;
	backgroundRect.right = screenWidth;
	backgroundRect.bottom = screenHeight;

	batch->Draw(background.Get(),backgroundRect,&bgSource);


	// größe der Spieler berechnen
	float playerWidth = PLAYER_WIDTH * screenWidth;
	float playerHeight = PLAYER_HEIGHT * screenHeight;

	// aktuelle Spieler Position in pixeln
	float topPlayerX = topPlayer->Position * screenWidth;
	float bottomPlayerX = bottomPlayer->Position * screenWidth;

	

	RECT playerSource;
	playerSource.left = bumpOffset;
	playerSource.top = 0;
	playerSource.right = bumpOffset + 80;
	playerSource.bottom = 10;

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
	batch->Draw(playerTexture.Get(),topPlayerRect,&playerSource);
	batch->Draw(playerTexture.Get(),bottomPlayerRect,&playerSource);

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
	float xPos = y / screenHeight;
	bottomPlayer->Intention = xPos;


	/*if(y >= 0.5)
	{
		bottomPlayer->Intention = x / screenWidth;
	}
	else
	{
		topPlayer->Intention = x / screenWidth;
	}*/
}
