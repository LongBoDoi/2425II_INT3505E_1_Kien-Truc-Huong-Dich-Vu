import Pyro4
import mysql.connector

@Pyro4.expose
class OrderService:
    def __init__(self):
        # Kết nối tới MySQL
        self.db = mysql.connector.connect(
            host="localhost",
            user="root",
            password="123456",
            database="ecommerce"
        )
        self.cursor = self.db.cursor()

    # Hàm tính số tiền
    def calculateTotal(self, product_id, quantity):
        self.cursor.execute("SELECT price FROM products WHERE product_id = %s", (product_id,))
        result = self.cursor.fetchone()

        if result:
            price = result[0]
            total = price * quantity
            return f"Đã xác nhận order: {quantity} x {product_id} = ${total:.2f}"
        else:
            return "Không tìm thấy sản phẩm"

# Chạy server
def start_server():
    daemon = Pyro4.Daemon(host="14.225.254.152")   # IP của server
    ns = Pyro4.locateNS()
    uri = daemon.register(OrderService)
    ns.register("orders.service", uri, safe=True)
    
    print(f"Service is running.... URI: @{uri}")
    daemon.requestLoop()

if __name__ == "__main__":
    start_server()
