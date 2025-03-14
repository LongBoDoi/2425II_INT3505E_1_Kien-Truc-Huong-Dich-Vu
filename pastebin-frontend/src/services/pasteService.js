const API_BASE_URL = import.meta.env.VITE_API_URL;

console.log(API_BASE_URL)
/**
 * Creates a new paste using FormData (multipart/form-data)
 * @param {string} content - The content of the paste
 * @param {string|null} expiresAt - Optional expiration date in YYYY-MM-DD format
 * @returns {Promise<string>} - The paste key
 */
export const createPaste = async (content, expiresAt = null, exposure, title) => {
  try {
    console.log(title);
    const formData = new FormData();
    formData.append("content", content);
    if (expiresAt) formData.append("expiresAt", expiresAt);
    formData.append("exposure", exposure == "Private" ? 1 : 0);
    formData.append("pasteName", title ? title : "Untitle" );
    const response = await fetch(`${API_BASE_URL}/Paste/CreatePaste`, {
      method: "POST",
      body: formData,
    });

    if (!response.ok) {
      throw new Error(`Error: ${response.status}`);
    }

    return await response.text(); // 🔹 API trả về key dưới dạng text
  } catch (error) {
    console.error("Error creating paste:", error);
    throw error;
  }
};

/**
 * Gets the content of a paste by its key
 * @param {string} pasteKey - The paste key
 * @returns {Promise<string>} - The paste content
 */
export const getPasteContent = async (pasteKey) => {


  try {
    const response = await fetch(
      `${API_BASE_URL}/Paste/GetPasteData?pasteKey=${pasteKey}`
    );

    if (!response.ok) {
      throw new Error(`Error: ${response.status}`);
    }

    return await response.text(); // 🔹 Trả về nội dung paste
  } catch (error) {
    console.error("Error fetching paste:", error);
    throw error;
  }
};

/**
 * Gets the 10 most recent pastes as raw text
 * @returns {Promise<string>} - The raw text response
 */
export const getRecentPastes = async () => {
  try {
    const response = await fetch(`${API_BASE_URL}/Paste/GetRecentPastes`);

    if (!response.ok) {
      throw new Error(`Error: ${response.status}`);
    }

    return await response.text(); // Trả về chuỗi text từ API
  } catch (error) {
    console.error("Error fetching recent pastes:", error);
    throw error;
  }
};

