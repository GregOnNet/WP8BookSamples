#pragma once

#include "Direct3DBase.h"
#include "SpriteBatch.h"
#include "Playground.h"


using namespace DirectX;

ref class PongRenderer sealed : public Direct3DBase
{
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> tex;
	std::unique_ptr<DirectX::SpriteBatch>  m_spriteBatch;
	Playground* playground;


public:
	PongRenderer();
	virtual void CreateDeviceResources() override;
	virtual void CreateWindowSizeDependentResources() override;
	virtual void Render() override;

	void Update(float timeTotal, float timeDelta);

	void OnPointerPressed(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::PointerEventArgs^ args);
	void OnPointerMoved(Windows::UI::Core::CoreWindow^ sender, Windows::UI::Core::PointerEventArgs^ args);

};
