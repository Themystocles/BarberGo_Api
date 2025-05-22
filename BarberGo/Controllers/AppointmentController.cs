using AutoMapper;
using BarberGo.DTOs;
using BarberGo.Entities;
using BarberGo.Entities.DTOs;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly GenericRepositoryServices<Appointment> _service;
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;

    public AppointmentController(GenericRepositoryServices<Appointment> service, IMapper mapper, IAppointmentRepository repository)
    {
        _service = service;
        _mapper = mapper;
        _repository = repository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AppointmentDto dto)
    {
        var entity = _mapper.Map<Appointment>(dto);
        var result = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    [HttpGet("Agendamentos")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointment()
    {
        var entities = await _service.GetList();
        return Ok(entities);
    }
  


    [HttpGet("find/{id}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        return Ok(entity);
    }
    [HttpGet("appointments/{iduser}")]
    public async Task <ActionResult<IEnumerable<MyAppointmentDto>>> GetAppointmenteByIdUser(int iduser)
    {
        var Appointment = await _repository.GetAppointmentsByUserId(iduser);

        return Ok(Appointment);
    }
    [HttpGet("Historyappointments/{iduser}")]
    public async Task<ActionResult<IEnumerable<MyAppointmentDto>>> GetHistorybyDateAndId(int iduser,[FromQuery] DateTime? date)
    {
        var Appointment = await _repository.GetHistoryAppointment(iduser, date);

        return Ok(Appointment);
    }


    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, AppointmentDto dto)
    {
        var entity = _mapper.Map<Appointment>(dto);
        entity.Id = id;
        var updated = await _service.UpdateAsync(entity);
        return Ok(updated);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
