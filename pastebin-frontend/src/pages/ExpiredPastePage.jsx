import { Link } from "react-router-dom";

const ExpiredPastePage = () => {
  return (
    <div style={styles.container}>
      <h1 style={styles.errorCode}>Paste Not Found</h1>
      <h2 style={styles.title}>The paste you are looking for doesn't exist or has expired.</h2>
      <p style={styles.description}>
        Please check the URL or create a new paste.
      </p>
      <Link to="/" style={styles.button}>
        Go Home
      </Link>
    </div>
  );
};

const styles = {
  container: {
    maxWidth: "800px",
    margin: "0 auto",
    textAlign: "center",
    padding: "48px 0",
  },
  errorCode: {
    fontSize: "32px",
    fontWeight: "bold",
    color: "#4F46E5", 
    marginBottom: "16px",
  },
  title: {
    fontSize: "24px",
    fontWeight: "600",
    marginBottom: "16px",
  },
  description: {
    color: "#6B7280", 
    marginBottom: "32px",
  },
  button: {
    display: "inline-block",
    padding: "8px 16px",
    backgroundColor: "#4F46E5", 
    color: "white",
    borderRadius: "6px",
    textDecoration: "none",
    transition: "background 0.2s",
  },
};

export default ExpiredPastePage;
