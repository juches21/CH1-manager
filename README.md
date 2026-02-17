

<h1>🎮 CH1 Manager</h1>
<img src="https://github.com/juches21/Juches21-images/blob/3f0353f32f6869ccbe967e5c59cc5116e9d3b131/ch1%20manager%20logo.png" alt="CH1 Manager Logo">

<p><strong>CH1 Manager</strong> is a game developed in <strong>Unity</strong> where players take on the role of a Formula 1 race engineer. You do not drive the car; <strong>you make the strategic decisions in real time</strong>, managing tires, race pace, and dynamic events.</p>
<p><strong>Final project for video game studies, developed entirely solo.</strong><br>Development duration: 3 months</p>

<hr>

<h2>📖 Description</h2>
<p>In <strong>CH1 Manager</strong>:</p>
<ul>
  <li>You control strategy, not the steering wheel.</li>
  <li>Every decision can take your driver to the podium—or to retirement.</li>
  <li>Manage tires, race pace, pit stops, and respond to radio events.</li>
  <li>Every lap is a decision, every pit stop a risk, every mistake has consequences.</li>
</ul>
<p>Victory depends not only on speed but on <strong>strategy and decision-making</strong>.</p>

<hr>

<h2>✨ Features</h2>
<ul>
  <li>Real-time race management</li>
  <li>Tire strategy and pit stop decisions</li>
  <li>Data-driven gameplay</li>
  <li>Risk vs reward mechanics</li>
  <li>Fast-paced strategic tension</li>
</ul>

<hr>

<h2>🎮 Gameplay / Screenshots</h2>
<img src="https://github.com/juches21/Juches21-images/blob/d4b73829c70c1fc0e7506b46620b8200baa1cb20/Ch1Captura1.PNG" alt="Screenshot 1">
<img src="https://github.com/juches21/Juches21-images/blob/d4b73829c70c1fc0e7506b46620b8200baa1cb20/Ch1Captura2.PNG" alt="Screenshot 2">
<img src="https://github.com/juches21/Juches21-images/blob/d4b73829c70c1fc0e7506b46620b8200baa1cb20/Ch1Captura3.PNG" alt="Screenshot 3">

<hr>

<h2>🛠️ Technologies Used</h2>
<ul>
  <li><strong>Engine:</strong> Unity 2022 LTS</li>
  <li><strong>Language:</strong> C#</li>
  <li><strong>Backend:</strong> JSON</li>
  <li><strong>Version control:</strong> GitHub</li>
</ul>

<hr>

<h2>🧠 Architecture / How It Works</h2>

<h3>Data_base.cs</h3>
<p>Manages game data loading from JSON files.</p>

<pre><code>{
  "pilotos": [
    {
      "nombre": "Alfred Garcia",
      "numero": 21,
      "escuderia": 1,
      "compuesto": "m",
      "desgaste": 100,
      "tiempo_total": 0,
      "tiempo_lap": 0,
      "modo": 2,
      "vuelta": 0,
      "casco": "sol_y_luna"
    }
  ]
}</code></pre>

<ul>
  <li>Loads information for drivers, tracks, teams, and radio stations.</li>
  <li>Converts JSON into serializable objects (<code>Driver</code>, <code>Track</code>, <code>Team</code>, <code>Radio</code>).</li>
  <li>Provides centralized access to loaded data for other scripts.</li>
</ul>

<h3>Manager.cs</h3>
<ul>
  <li>Main race logic: lap counting, lap times, pit stops, AI updates, and events.</li>
  <li>Tracks pilot states in real time: lap time, total time, tire wear, driving mode.</li>
  <li>Applies penalties/advantages based on errors and driving mode.</li>
  <li>Manages AI pilot pace dynamically.</li>
  <li>Updates UI: time table, podium panel, lap indicators.</li>
</ul>

<h3>PitStop.cs</h3>
<ul>
  <li>Simulates pit stop interactions.</li>
  <li>Manages mechanic buttons, tire changes, and animations.</li>
  <li>Controls audio feedback (car and guns).</li>
  <li>Calculates and applies pit stop time to the player.</li>
</ul>

<h3>Player.cs</h3>
<ul>
  <li>Controls the player’s pilot: UI, driving modes, tire strategy, pit stops, and lap updates.</li>
  <li>Driving modes: Eco, Normal, Aggressive.</li>
  <li>Integrates with Manager, PitStop, and Data_base scripts.</li>
</ul>

<h3>RadioManager.cs</h3>
<ul>
  <li>Manages radio messages and car faults.</li>
  <li>Generates random messages: problems, neutral, or positive.</li>
  <li>Updates fault counters and UI indicators (green/yellow/red).</li>
  <li>Synchronizes with Player and pit stop system.</li>
</ul>

<h3>TimeTable.cs</h3>
<ul>
  <li>Displays and updates the race time table in the UI.</li>
  <li>Supports different display modes: leader delta, previous pilot delta, tire wear, tire compound.</li>
  <li>Shows team logos and colors loaded from JSON.</li>
</ul>

<h3>TyreWearManager.cs</h3>
<ul>
  <li>Manages tire wear per lap for all drivers.</li>
  <li>Wear depends on compound (<code>s</code>, <code>m</code>, <code>h</code>) and driving mode (Eco, Normal, Aggressive).</li>
  <li>Impacts pit stop strategy and lap times.</li>
</ul>

<hr>

<h2>⚙️ Main Interaction Flow</h2>
<pre><code>
Player → PitStop → TyreWearManager → Manager → TimeTable → RadioManager
</code></pre>
<ul>
  <li>Lap times and pilot state update each lap.</li>
  <li>Radio events and decisions directly affect pilot performance.</li>
  <li>Time table reflects race progress, tire state, and chosen
