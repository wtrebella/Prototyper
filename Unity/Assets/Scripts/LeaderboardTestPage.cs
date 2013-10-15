using UnityEngine;
using System.Collections;

public class LeaderboardTestPage : AbstractPage {
	FLabel timeLabel;
	FLabel clickLabel;
	float timeLeft = 1;
	int clicks = 0;
	bool hasStartedClicking = false;
	FTextbox nameTextBox;

	public LeaderboardTestPage() {
		FSprite background = new FSprite("WhiteBox");
		background.width = Futile.screen.width;
		background.height = Futile.screen.height;
		background.color = Color.black;
		background.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
		AddChild(background);

		FLabel titleLabel = new FLabel("Franchise", "Click as many times as you can!");
		titleLabel.scale = 0.8f;
		titleLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight + 100);
		AddChild(titleLabel);

		timeLabel = new FLabel("Franchise", "Time Left: " + timeLeft.ToString("0.00"));
		timeLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight - 100);
		timeLabel.scale = 0.8f;
		AddChild(timeLabel);

		clickLabel = new FLabel("Franchise", clicks.ToString());
		clickLabel.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
		AddChild(clickLabel);

		ListenForUpdate(HandleUpdate);
	}

	public void TextBoxReturnPressed(FTextbox textBox) {
		nameTextBox.isVisible = false;
		HighScore hs = GameObject.Find("Futile").GetComponent<HighScore>();
		hs.SetName(nameTextBox.Text);
		hs.Setscore(clicks);
		hs.StartProcess();

	}

	public void HandleUpdate() {
		if (!hasStartedClicking && Input.GetMouseButtonDown(0)) hasStartedClicking = true;

		if (timeLeft <= 0 || !hasStartedClicking) return;

		timeLeft -= Time.deltaTime;

		if (timeLeft >= 0) {
			if (Input.GetMouseButtonDown(0)) clicks++;
		}
		else {
			timeLeft = 0;
			nameTextBox = new FTextbox("textBoxBackground", "caret", "Franchise", Color.black);
			nameTextBox.MaxLength = 50;
			nameTextBox.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
			nameTextBox.SignalReturnPressed += TextBoxReturnPressed;
			nameTextBox.Focus();
			AddChild(nameTextBox);
		}

		timeLabel.text = "Time Left: " + timeLeft.ToString("0.00");
		clickLabel.text = clicks.ToString();
	}
}
