import { Link } from "react-router-dom";

const NotFoundPage = () => {
  return (
    <div style={styles.container}>
      <h1 style={styles.errorCode}>404</h1>
      <h2 style={styles.title}>Page Not Found</h2>
      <p style={styles.description}>
        The page you are looking for doesn't exist or has been moved.
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
    fontSize: "72px",
    fontWeight: "bold",
    color: "#4F46E5", // Indigo-600
    marginBottom: "16px",
  },
  title: {
    fontSize: "24px",
    fontWeight: "600",
    marginBottom: "16px",
  },
  description: {
    color: "#6B7280", // Gray-600
    marginBottom: "32px",
  },
  button: {
    display: "inline-block",
    padding: "8px 16px",
    backgroundColor: "#4F46E5", // Indigo-600
    color: "white",
    borderRadius: "6px",
    textDecoration: "none",
    transition: "background 0.2s",
  },
};

export default NotFoundPage;
