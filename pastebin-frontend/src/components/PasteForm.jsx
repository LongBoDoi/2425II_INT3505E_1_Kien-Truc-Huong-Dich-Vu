"use client";

import { useState } from "react";

const PasteForm = ({ onSubmit, isLoading }) => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [expiresAt, setExpiresAt] = useState("never");
  const [exposure, setExposure] = useState("Public");

  const handleSubmit = (e) => {
    e.preventDefault();
    const currentTime = new Date();
    let expiryTime = null;

    switch (expiresAt) {
      case "1m":
        expiryTime = new Date(currentTime.getTime() + 1 * 60000).toISOString();
        break;
      case "1h":
        expiryTime = new Date(
          currentTime.getTime() + 1 * 3600000
        ).toISOString();
        break;
      case "1d":
        expiryTime = new Date(
          currentTime.getTime() + 24 * 3600000
        ).toISOString();
        break;
      case "1mo":
        expiryTime = new Date(
          currentTime.getTime() + 30 * 24 * 3600000
        ).toISOString();
        break;
      default:
        expiryTime = null;
    }

    onSubmit(content, expiryTime, exposure, title);
  };

  return (
    <form onSubmit={handleSubmit} style={styles.form}>
      {/* Title input */}
      <div>
        <label htmlFor="title" style={styles.label}>
          Title
        </label>
        <input
          id="title"
          type="text"
          style={styles.input}
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
      </div>

      {/* Content input */}
      <div>
        <label htmlFor="content" style={styles.label}>
          Paste Content
        </label>
        <textarea
          id="content"
          rows={12}
          style={styles.textarea}
          value={content}
          onChange={(e) => setContent(e.target.value)}
          required
        />
      </div>

      {/* Expiration date input */}
      <div>
        <label htmlFor="expiresAt" style={styles.label}>
          Expiration Time
        </label>
        <select
          id="expiresAt"
          style={styles.input}
          value={expiresAt}
          onChange={(e) => setExpiresAt(e.target.value)}
        >
          <option value="1h">1 Hour</option>
          <option value="1m">1 Minute</option>
          <option value="1d">1 Day</option>
          <option value="1mo">1 Month</option>
          <option value="never">Never</option>
        </select>
      </div>

      {/* Exposure input */}
      <div>
        <label htmlFor="exposure" style={styles.label}>
          Exposure
        </label>
        <select
          id="exposure"
          style={styles.input}
          value={exposure}
          onChange={(e) => setExposure(e.target.value)}
        >
          <option value="Public">Public</option>
          <option value="Private">Private</option>
        </select>
      </div>

      {/* Submit button */}
      <div style={styles.buttonContainer}>
        <button
          type="submit"
          disabled={isLoading}
          style={{
            ...styles.button,
            ...(isLoading ? styles.buttonDisabled : {}),
          }}
        >
          {isLoading ? "Creating..." : "Create Paste"}
        </button>
      </div>
    </form>
  );
};

const styles = {
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "16px",
  },
  label: {
    display: "block",
    fontSize: "14px",
    fontWeight: "500",
    marginBottom: "4px",
    color: "#374151",
  },
  textarea: {
    width: "100%",
    padding: "8px",
    border: "1px solid #D1D5DB",
    borderRadius: "6px",
    boxShadow: "0px 1px 3px rgba(0, 0, 0, 0.1)",
    fontSize: "14px",
    outline: "none",
    transition: "border 0.2s",
  },
  input: {
    width: "100%",
    padding: "8px",
    border: "1px solid #D1D5DB",
    borderRadius: "6px",
    boxShadow: "0px 1px 3px rgba(0, 0, 0, 0.1)",
    fontSize: "14px",
    outline: "none",
  },
  buttonContainer: {
    display: "flex",
    justifyContent: "flex-end",
  },
  button: {
    padding: "8px 16px",
    backgroundColor: "#4F46E5",
    color: "white",
    borderRadius: "6px",
    border: "none",
    cursor: "pointer",
    transition: "background 0.2s",
  },
  buttonDisabled: {
    opacity: "0.75",
    cursor: "not-allowed",
  },
};

export default PasteForm;
