import { Link } from "react-router-dom";

const Navbar = () => {
  return (
    <nav style={styles.navbar}>
      <div style={styles.container}>
        <Link to="/" style={styles.logo}>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            style={styles.icon}
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
            />
          </svg>
          PasteBin
        </Link>
        <div>
          <Link to="/" style={styles.link}>
            New Paste
          </Link>
        </div>
      </div>
    </nav>
  );
};

const styles = {
  navbar: {
    backgroundColor: "#4f46e5", // bg-indigo-600
    color: "white",
    boxShadow: "0px 4px 6px rgba(0, 0, 0, 0.1)", // shadow-md
    padding: "12px 16px",
  },
  container: {
    maxWidth: "1200px",
    margin: "0 auto",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
  },
  logo: {
    display: "flex",
    alignItems: "center",
    fontSize: "1.25rem",
    fontWeight: "bold",
    textDecoration: "none",
    color: "white",
  },
  icon: {
    width: "24px",
    height: "24px",
    marginRight: "8px",
  },
  link: {
    padding: "8px 16px",
    borderRadius: "4px",
    textDecoration: "none",
    color: "white",
    transition: "background-color 0.3s",
    cursor: "pointer",
  },
};

// Hover effect cho inline style
styles.link["&:hover"] = { backgroundColor: "#4338ca" }; // hover:bg-indigo-700

export default Navbar;
