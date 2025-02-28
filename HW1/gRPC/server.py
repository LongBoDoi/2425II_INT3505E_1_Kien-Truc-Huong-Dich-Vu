import grpc
import mysql.connector
import order_service_pb2
import order_service_pb2_grpc
from concurrent import futures

class OrderService(order_service_pb2_grpc.OrderServiceServicer):
    def __init__(self):
        # Kết nối tới MySQL
        self.db = mysql.connector.connect(
            host="127.0.0.1",
            user="root",
            password="123456",
            database="ecommerce"
        )
        self.cursor = self.db.cursor()

    # Hàm tính giá trị đơn hàng
    def CalculateTotal(self, request, context):
        product_id = request.product_id
        quantity = request.quantity

        self.cursor.execute("SELECT price FROM products WHERE product_id = %s", (product_id,))
        result = self.cursor.fetchone()

        if result:
            price = result[0]
            total = price * quantity
            message = f"Đã xác nhận order: {quantity} x {product_id} = ${total:.2f}"
        else:
            message = "Không tìm thấy sản phẩm"

        return order_service_pb2.OrderResponse(message=message)

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    order_service_pb2_grpc.add_OrderServiceServicer_to_server(OrderService(), server)

    server.add_insecure_port("192.168.211.129:50051")   # IP máy chủ
    print("gRPC Server running on port 50051...")

    server.start()
    server.wait_for_termination()

if __name__ == "__main__":
    serve()
