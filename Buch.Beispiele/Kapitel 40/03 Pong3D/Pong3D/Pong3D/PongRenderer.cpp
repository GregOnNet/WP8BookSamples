#include "pch.h"
#include "PongRenderer.h"

#include "DDSTextureLoader.h"

XMFLOAT2 position;
XMFLOAT2 velocity;

PongRenderer::PongRenderer()
{

}

void PongRenderer::CreateDeviceResources()
{
	Direct3DBase::CreateDeviceResources();

	m_spriteBatch = std::unique_ptr<SpriteBatch>(new SpriteBatch(m_d3dContext.Get()));

	HRESULT hr = CreateDDSTextureFromFile(m_d3dDevice.Get(),L"chrome.dds",NULL,tex.GetAddressOf(),NULL);

	position = XMFLOAT2(240,400);
	velocity = XMFLOAT2(100,100);
}


void PongRenderer::CreateWindowSizeDependentResources()
{
	Direct3DBase::CreateWindowSizeDependentResources();

	playground = new Playground(m_d3dDevice,m_d3dContext);
	playground->Init(ConvertDipsToPixels(m_windowBounds.Width),ConvertDipsToPixels(m_windowBounds.Height));
}

void PongRenderer::Update(float timeTotal, float timeDelta)
{
	// Spielfeld aktualisieren
	playground->Update(timeTotal,timeDelta);
}


void PongRenderer::Render()
{
	// Bildschirm löschen
	const float midnightBlue[] = { 0.098f, 0.048f, 0.439f, 1.000f };
	m_d3dContext->ClearRenderTargetView(m_renderTargetView.Get(),midnightBlue);
	m_d3dContext->ClearDepthStencilView(m_depthStencilView.Get(),D3D11_CLEAR_DEPTH, 1.0f, 0);
	m_d3dContext->OMSetRenderTargets(1,m_renderTargetView.GetAddressOf(), m_depthStencilView.Get());

	
	m_spriteBatch->Begin();
	playground->Render3D(m_spriteBatch.get());
	m_spriteBatch->End();

	
}

void PongRenderer::OnPointerMoved(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::PointerEventArgs^ args)
{
	float realX = ConvertDipsToPixels(args->CurrentPoint->Position.X);
	float realY = ConvertDipsToPixels(args->CurrentPoint->Position.Y);
	playground->HandleTouch(realX,realY);
}

void PongRenderer::OnPointerPressed(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::PointerEventArgs^ args)
{
	float realX = ConvertDipsToPixels(args->CurrentPoint->Position.X);
	float realY = ConvertDipsToPixels(args->CurrentPoint->Position.Y);
	playground->HandleTouch(realX,realY);
}


