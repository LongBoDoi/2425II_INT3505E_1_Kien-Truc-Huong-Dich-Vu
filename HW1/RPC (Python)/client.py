import Pyro4

# Dùng IP của máy server
server_ip = "14.225.254.152"

ns = Pyro4.locateNS(host=server_ip)  # Tìm tên server Pyro
uri = ns.lookup("orders.service")  # Lấy uri service
order_service = Pyro4.Proxy(uri)  # Connect to service

# Gọi tới server
product_id = "P0001"
quantity = 3
response = order_service.calculateTotal(product_id, quantity)
print(response)