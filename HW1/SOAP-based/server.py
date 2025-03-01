import mysql.connector
# Kết nối MySQL
def get_db_connection():
    return mysql.connector.connect(
        host="localhost",
        user="root",
        password="123456",
        database="ecommerce"
    )

from flask import Flask, request, Response, send_from_directory
import os

app = Flask(__name__)

# Serve file WSDL
@app.route('/order-service', methods=['GET'])
def serve_wsdl():
    return send_from_directory(os.path.dirname(__file__), 'order_service.wsdl', mimetype='text/xml')

# Xử lý SOAP request
@app.route('/order-service', methods=['POST'])
def soap_service():
    # Giải mã SOAP request
    soap_request = request.data.decode('utf-8')
    
    # Lấy param từ SOAP request
    product_id = str(soap_request.split("<productId>")[1].split("</productId>")[0])
    quantity = int(soap_request.split("<quantity>")[1].split("</quantity>")[0])
    
    # Lấy dữ liệu từ db
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT price FROM products WHERE product_id = %s", (product_id,))
    result = cursor.fetchone()
    
    if result:
        price = result[0]
        total_cost = price * quantity
        soap_response = f"""
        <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
            <soap:Body>
                <processOrderResponse>
                    <orderStatus>Đã xác nhận order: {quantity} * {product_id} = ${total_cost}</orderStatus>
                </processOrderResponse>
            </soap:Body>
        </soap:Envelope>
        """
    else:
        soap_response = f"""
        <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
            <soap:Body>
                <processOrderResponse>
                    <error>Không tìm thấy sản phẩm</error>
                </processOrderResponse>
            </soap:Body>
        </soap:Envelope>
        """
    
    return Response(soap_response, mimetype='text/xml')

if __name__ == '__main__':
    app.run(host='192.168.211.129', port=5000)