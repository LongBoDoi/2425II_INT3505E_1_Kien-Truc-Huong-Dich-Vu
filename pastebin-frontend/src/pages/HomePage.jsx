"use client";

import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import PasteForm from "../components/PasteForm";
import { createPaste, getRecentPastes } from "../services/pasteService";
import { Link } from "react-router-dom"; // Import Link
import { addViewAnalytic, getViewAnalytic } from "../services/pasteService";
const SERVICE_KEY = "serviceCalled";

const HomePage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [recentPastes, setRecentPastes] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRecentPastes = async () => {
      try {
        const pastesText = await getRecentPastes();
        const pastesArray = pastesText
          .split(/\|pasteKey=/)
          .map((entry, index) => {
            if (index > 0) entry = "pasteKey=" + entry;

            const match = entry.match(
              /pasteKey=([^;]+);createdAt=([^;]+);pasteName=(.+)/
            );

            if (match) {
              return {
                pasteKey: match[1],
                createdAt: match[2],
                pasteName: match[3],
              };
            }
            return null;
          })
          .filter(Boolean);
        setRecentPastes(pastesArray);
      } catch (error) {
        console.error("Error fetching recent pastes:", error);
      }
    };
    if (!sessionStorage.getItem(SERVICE_KEY)) {
      addViewAnalytic();
      sessionStorage.setItem(SERVICE_KEY, "true");
    }
    fetchRecentPastes();
  }, []);

  const handleSubmit = async (content, expiresAt, exposure, title) => {
    if (!content.trim()) {
      toast.error("Paste content cannot be empty");
      return;
    }

    setIsLoading(true);
    try {
      const pasteKey = await createPaste(content, expiresAt, exposure, title);
      toast.success("Paste created successfully!");
      navigate(`/paste/${pasteKey}`);
    } catch (error) {
      console.error("Error creating paste:", error);
      toast.error("Failed to create paste. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      {/* Form tạo paste */}
      <div style={styles.mainContent}>
        <div style={styles.card}>
          <h1 style={styles.title}>Create New Paste</h1>
          <PasteForm onSubmit={handleSubmit} isLoading={isLoading} />
        </div>
      </div>

      {/* Recent Pastes */}
      <div style={styles.recentPastes}>
        <h2 style={styles.recentTitle}>Recent Pastes</h2>
        {recentPastes.map((paste) => (
          <Link
            to={`/paste/${paste.pasteKey}`}
            key={paste.pasteKey}
            style={styles.pasteLink}
          >
            <div style={styles.pasteCard}>
              <strong>{paste.pasteName || "Untitled"}</strong>
              <span style={styles.timestamp}>{paste.createdAt}</span>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
};

const styles = {
  container: {
    display: "flex",
    justifyContent: "space-between",
    maxWidth: "1000px",
    margin: "0 auto",
    padding: "24px",
    position: "relative",
  },
  mainContent: {
    flex: 1,
    maxWidth: "600px",
  },
  card: {
    backgroundColor: "white",
    borderRadius: "8px",
    boxShadow: "0px 4px 6px rgba(0, 0, 0, 0.1)",
    padding: "24px",
  },
  title: {
    fontSize: "24px",
    fontWeight: "bold",
    marginBottom: "24px",
  },
  recentPastes: {
    position: "absolute",
    right: "0",
    top: "24px",
    width: "280px",
  },
  pasteLink: {
    textDecoration: "none",
    color: "black",
  },
  pasteCard: {
    width: "100%",
    backgroundColor: "#fff",
    padding: "14px",
    borderRadius: "6px",
    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
    marginBottom: "16px", // Tăng khoảng cách giữa các Paste Cards
    borderBottom: "2px solid #ddd", // Thêm border-bottom để phân tách rõ ràng
    transition: "background-color 0.2s",
    cursor: "pointer",
  },
  pasteCardHover: {
    backgroundColor: "#f7f7f7",
  },
  timestamp: {
    display: "block",
    fontSize: "12px",
    color: "#6B7280",
    marginTop: "4px",
  },
  recentTitle: {
    fontSize: "24px",
    fontWeight: "bold",
    marginBottom: "12px",
    color: "#374151",
  },
};

export default HomePage;
