"use client";

import { useState, useEffect } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import { getPasteContent } from "../services/pasteService";
const API_BASE_URL = "http://localhost:5173";
const ViewPastePage = () => {
  const { pasteKey } = useParams();
  const navigate = useNavigate();
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [createdAt, setCreatedAt] = useState("");
  const [views, setViews] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPaste = async () => {
      try {
        const pasteData = await getPasteContent(pasteKey);
        // Kiểm tra nếu paste không tồn tại hoặc đã hết hạn
        if (
          !pasteData ||
          pasteData.trim() === "Mã paste không tồn tại hoặc đã hết hạn"
        ) {
          navigate("/expired-paste");
          return;
        }
        const extractField = (data, field) => {
          const regex = new RegExp(
            `${field}=(.*?)(?=;(?:content|createdAt|views|pasteName)=|$)`,
            "s"
          );
          const match = data.match(regex);
          return match ? match[1].trim() : null;
        };

        const dataParts = {
          content: extractField(pasteData, "content"),
          createdAt: extractField(pasteData, "createdAt"),
          views: extractField(pasteData, "views"),
          pasteName: extractField(pasteData, "pasteName"),
        };

        console.log(dataParts);

        console.log(dataParts);
        setTitle(dataParts.pasteName || "Untitled");
        setContent(dataParts.content || "");
        setCreatedAt(
          new Date(dataParts.createdAt + "Z").toLocaleDateString("vi-VN")
        );
        setViews(Number(dataParts.views) || 0);
      } catch (err) {
        console.error("Error fetching paste:", err);
        navigate("/paste-not-found");
      } finally {
        setIsLoading(false);
      }
    };

    fetchPaste();
  }, [pasteKey, navigate]);

  const copyToClipboardFallback = (text) => {
    const textarea = document.createElement("textarea");
    textarea.value = text;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand("copy");
    document.body.removeChild(textarea);
    toast.success("Copied to clipboard!");
  };

  const copyToClipboard = () => {
    if (navigator.clipboard) {
      navigator.clipboard
        .writeText(window.location.href)
        .then(() => toast.success("Copied to clipboard!"))
        .catch(() =>
          copyToClipboardFallback(window.location.href)
        );
    } else {
      copyToClipboardFallback(window.location.href);
    }
  };

  if (isLoading) {
    return (
      <div style={styles.container}>
        <div style={styles.loading}>
          <div style={styles.loadingText}></div>
          <div style={styles.loadingText}></div>
          <div style={styles.loadingText}></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div style={styles.container}>
        <div style={styles.errorBox}>
          <h2 style={styles.errorTitle}>Paste Not Found</h2>
          <p style={styles.errorMessage}>{error}</p>
          <Link to="/" style={styles.button}>
            Create New Paste
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div style={styles.container}>
      <div style={styles.contentBox}>
        <div style={styles.header}>
          <h1 style={styles.title}>
            {title ? `${title} (${pasteKey})` : `Paste: ${pasteKey}`}
          </h1>

          <p style={styles.info}>
            Created At: {createdAt}{" "}
            <span style={{ marginLeft: "50px" }}>Views: {views}</span>
          </p>

          <div style={styles.buttonGroup}>
            <button onClick={copyToClipboard} style={styles.copyButton}>
              Copy
            </button>
            <Link to="/" style={styles.button}>
              New Paste
            </Link>
          </div>
        </div>
        <div style={styles.contentArea}>{content}</div>
      </div>
    </div>
  );
};

const styles = {
  container: {
    maxWidth: "800px",
    margin: "0 auto",
    padding: "24px",
  },
  contentBox: {
    backgroundColor: "white",
    borderRadius: "8px",
    boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
    padding: "24px",
  },
  header: {
    display: "flex",
    flexDirection: "column",
    gap: "8px",
    marginBottom: "16px",
  },
  title: {
    fontSize: "22px",
    fontWeight: "bold",
  },
  info: {
    marginTop: "-10px",
    fontSize: "14px",
    color: "#6B7280",
  },
  buttonGroup: {
    display: "flex",
    gap: "8px",
    justifyContent: "flex-end",
  },
  button: {
    padding: "8px 16px",
    backgroundColor: "#4F46E5",
    color: "white",
    borderRadius: "6px",
    textDecoration: "none",
    transition: "background 0.2s",
  },
  copyButton: {
    padding: "8px 16px",
    backgroundColor: "#E5E7EB",
    color: "#374151",
    borderRadius: "6px",
    cursor: "pointer",
  },
  contentArea: {
    border: "1px solid #D1D5DB",
    borderRadius: "6px",
    padding: "16px",
    backgroundColor: "#F9FAFB",
    whiteSpace: "pre-wrap",
    wordBreak: "break-word",
  },
  errorBox: {
    textAlign: "center",
    padding: "32px",
    backgroundColor: "white",
    borderRadius: "8px",
    boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
  },
  errorTitle: {
    fontSize: "18px",
    fontWeight: "bold",
    marginBottom: "8px",
  },
  errorMessage: {
    color: "#6B7280",
    marginBottom: "16px",
  },
  loading: {
    display: "flex",
    flexDirection: "column",
    gap: "8px",
    padding: "24px",
    backgroundColor: "#F3F4F6",
    borderRadius: "8px",
  },
  loadingText: {
    height: "12px",
    backgroundColor: "#E5E7EB",
    borderRadius: "4px",
  },
};

export default ViewPastePage;
