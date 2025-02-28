from locust import HttpUser, task, between, events
from thrift.transport import TSocket, TTransport
from thrift.protocol import TBinaryProtocol

import sys, os
gen_py_path = os.path.abspath(os.path.join(os.path.dirname(__file__), 'gen-py'))
sys.path.append(gen_py_path)
from order_service import OrderService # type: ignore

import random, time

server_ip="192.168.211.129" # IP server
class ThriftClient:
    def __init__(self, host=server_ip, port=9090):
        self.transport = TSocket.TSocket(host, port)
        self.transport = TTransport.TBufferedTransport(self.transport)
        self.protocol = TBinaryProtocol.TBinaryProtocol(self.transport)
        self.client = OrderService.Client(self.protocol)
        self.transport.open()

    def call_service(self):
        # Random sản phẩm, số lượng
        product_id = f"P{random.randint(1, 1000):04d}" 
        quantity = random.randint(1, 10)

        start_time = time.time()
        try:
            response = self.client.calculateTotal(product_id, quantity)

            total_time = int((time.time() - start_time) * 1000)
            events.request.fire(
                request_type="gRPC",
                name="calculateTotal",
                response_time=total_time,
                response_length=len(response)
            )
            
            print(response)
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            events.request.fire(
                request_type="gRPC",
                name="calculateTotal",
                response_time=total_time,
                exception=e
            )

    def close(self):
        self.transport.close()

class ThriftUser(HttpUser):
    wait_time = between(1, 2)  # Thời gian chờ giữa mỗi request

    @task
    def test_thrift_call(self):
        client = ThriftClient()
        client.call_service()  # Gọi service Thrift
        client.close()
