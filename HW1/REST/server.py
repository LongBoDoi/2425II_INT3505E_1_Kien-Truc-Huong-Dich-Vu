from flask import Flask, request, jsonify
import mysql.connector

app = Flask(__name__)

# Kết nối database
def get_db_connection():
    return mysql.connector.connect(
        host="localhost",
        user="root",
        password="123456",
        database="ecommerce"
    )

# REST API endpoint tạo order
@app.route('/CreateOrder', methods=['POST'])
def process_order():
    # Giải mã JSON request
    data = request.json
    product_id = data.get('product_id')
    quantity = data.get('quantity')
    
    if not product_id or not quantity:
        return jsonify({"error": "product_id và quantity không được bỏ trống"}), 400
    
    # Lấy dữ liệu từ database
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT price FROM products WHERE product_id = %s", (product_id,))
    result = cursor.fetchone()
    
    if result:
        price = result[0]
        total_cost = price * quantity
        return jsonify({"success": True, "message": f"Đã xác nhận order: {quantity} * {product_id} = ${total_cost}"})
    else:
        return jsonify({"success": False, "message": "Không tìm thấy sản phẩm"}), 404

if __name__ == '__main__':
    app.run(host='192.168.211.129', port=5000)