from thrift.transport import TSocket, TTransport
from thrift.protocol import TBinaryProtocol
from thrift.server import TServer
import mysql.connector

import sys, os
gen_py_path = os.path.abspath(os.path.join(os.path.dirname(__file__), 'gen-py'))
sys.path.append(gen_py_path)
from order_service import OrderService # type: ignore

class OrderHandler(OrderService.Iface):
    def __init__(self):
        self.db = mysql.connector.connect(
            host="127.0.0.1",
            user="root",
            password="123456",
            database="ecommerce"
        )
        self.cursor = self.db.cursor()

    def calculateTotal(self, product_id, quantity):
        self.cursor.execute("SELECT price FROM products WHERE product_id = %s", (product_id,))
        result = self.cursor.fetchone()

        if result:
            price = result[0]
            total = price * quantity
            return f"Đã xác nhận order: {quantity} x {product_id} = ${total:.2f}"
        else:
            return "Không tìm thấy sản phẩm"

def start_server():
    handler = OrderHandler()
    processor = OrderService.Processor(handler)
    transport = TSocket.TServerSocket(host="192.168.211.129", port=9090)    # IP server
    tfactory = TTransport.TBufferedTransportFactory()
    pfactory = TBinaryProtocol.TBinaryProtocolFactory()

    server = TServer.TSimpleServer(processor, transport, tfactory, pfactory)
    print("Thrift server running on port 9090...")
    server.serve()

if __name__ == "__main__":
    start_server()
