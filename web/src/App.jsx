import { useEffect, useState } from "react";

const API = import.meta.env.VITE_API_URL;

function App() {
  const [items, setItems] = useState([]);
  const [status, setStatus] = useState("loading");
  const [error, setError] = useState("");

  useEffect(() => {
    fetch(`${API}/api/menu`)
      .then((r) => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.json();
      })
      .then((data) => {
        setItems(data);
        setStatus("ok");
      })
      .catch((e) => {
        setError(e.message);
        setStatus("error");
      });
  }, []);

  return (
    <div style={{ maxWidth: 480, margin: "40px auto", fontFamily: "sans-serif" }}>
      <h1>🍽️ CampusEats</h1>
      <p style={{ fontSize: 12, color: "#888" }}>API: {API || "not set"}</p>

      {status === "loading" && <p>Loading menu… (the free API may take up to 60s to wake up)</p>}
      {status === "error" && <p style={{ color: "crimson" }}>Failed to load menu: {error}</p>}
      {status === "ok" && (
        <ul>
          {items.map((m) => (
            <li key={m.id}>
              {m.name} — Rs. {m.price}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default App;